import { loadGoogleMapsScript, createMap } from '../shared/google-maps.js';

var mapEl = document.getElementById('searchMap');
var canvasEl = document.getElementById('searchMapCanvas');
var resultsEl = document.getElementById('searchResults');

if (mapEl && canvasEl && resultsEl) {
    var boundsKeys = ['north', 'south', 'east', 'west'];
    var boundsInputs = {
        north: document.getElementById('northInput'),
        south: document.getElementById('southInput'),
        east: document.getElementById('eastInput'),
        west: document.getElementById('westInput')
    };
    var destinationInput = document.getElementById('destinationInput');
    var categoryLinks = document.querySelectorAll('.category-strip__item');

    var markersBySlug = {};
    var openSlug = null;
    var fetchController = null;
    var refreshTimer = null;
    var initialMapFitDone = false;

    function readMarkers() {
        var dataEl = document.getElementById('searchMapData');
        return dataEl ? JSON.parse(dataEl.textContent) : [];
    }

    function paramsWithoutBounds(source) {
        var params = new URLSearchParams();
        source.forEach(function (value, key) {
            if (boundsKeys.indexOf(key.toLowerCase()) === -1) params.append(key, value);
        });
        return params;
    }

    function paramsWithBounds(source, bounds) {
        var params = paramsWithoutBounds(source);
        boundsKeys.forEach(function (key) { params.set(key, bounds[key]); });
        return params;
    }

    function readUrlBounds() {
        var bounds = {};
        new URLSearchParams(window.location.search).forEach(function (value, key) {
            var name = key.toLowerCase();
            if (boundsKeys.indexOf(name) !== -1) bounds[name] = parseFloat(value);
        });

        var isValid = boundsKeys.every(function (key) { return typeof bounds[key] === 'number' && !isNaN(bounds[key]); });
        return isValid ? bounds : null;
    }

    function syncSearchBar(bounds) {
        boundsKeys.forEach(function (key) {
            if (boundsInputs[key]) boundsInputs[key].value = bounds[key];
        });

        categoryLinks.forEach(function (link) {
            var url = new URL(link.href, window.location.href);
            url.search = paramsWithBounds(url.searchParams, bounds).toString();
            link.href = url.toString();
        });
        
        if (destinationInput) {
            destinationInput.value = '';
            destinationInput.placeholder = destinationInput.dataset.areaPlaceholder;
        }
    }

    if (destinationInput) {
        destinationInput.addEventListener('input', function () {
            boundsKeys.forEach(function (key) {
                if (boundsInputs[key]) boundsInputs[key].value = '';
            });
            destinationInput.placeholder = destinationInput.dataset.defaultPlaceholder;
        });
    }

    loadGoogleMapsScript(mapEl.dataset.apiKey).then(function () {
        var map = createMap(canvasEl, { lat: 0, lng: 0 }, { scrollwheel: true });
        var infoWindow = new google.maps.InfoWindow();

        function closePopup() {
            infoWindow.close();
            openSlug = null;
        }

        function openPopup(slug, advancedMarker) {
            var card = resultsEl.querySelector('.property-card[data-property-slug="' + slug + '"]');
            if (!card) return;

            var popupCard = card.cloneNode(true);
            popupCard.classList.add('map-popup-card');

            var imageWrap = popupCard.querySelector('.property-card__image-wrap');
            if (imageWrap) {
                var closeBtn = document.createElement('button');
                closeBtn.type = 'button';
                closeBtn.className = 'map-popup-card__close';
                closeBtn.textContent = '×';
                closeBtn.addEventListener('click', function (e) {
                    e.stopPropagation();
                    closePopup();
                });
                imageWrap.appendChild(closeBtn);
            }

            infoWindow.setContent(popupCard);
            infoWindow.open({ map: map, anchor: advancedMarker });
            openSlug = slug;
        }

        function createMarker(item) {
            var pin = document.createElement('div');
            pin.className = 'map-price-pin';
            pin.textContent = '$' + Math.round(item.pricePerNight);

            var advancedMarker = new google.maps.marker.AdvancedMarkerElement({
                position: { lat: item.latitude, lng: item.longitude },
                map: map,
                content: pin
            });

            pin.addEventListener('click', function () { openPopup(item.slug, advancedMarker); });

            return { marker: advancedMarker, pin: pin };
        }

        function syncMarkers(items) {
            var seen = {};

            items.forEach(function (item) {
                if (item.latitude == null || item.longitude == null) return;

                seen[item.slug] = true;
                if (!markersBySlug[item.slug]) markersBySlug[item.slug] = createMarker(item);
            });

            Object.keys(markersBySlug).forEach(function (slug) {
                if (seen[slug]) return;

                markersBySlug[slug].marker.map = null;
                delete markersBySlug[slug];
                if (openSlug === slug) closePopup();
            });
        }

        function wireCards() {
            resultsEl.querySelectorAll('.property-card').forEach(function (card) {
                var entry = markersBySlug[card.dataset.propertySlug];
                if (!entry) return;

                card.addEventListener('mouseenter', function () { entry.pin.classList.add('is-active'); });
                card.addEventListener('mouseleave', function () { entry.pin.classList.remove('is-active'); });
            });
        }

        function refreshResults(bounds) {
            if (fetchController) fetchController.abort();
            fetchController = new AbortController();

            var params = paramsWithBounds(new URLSearchParams(window.location.search), bounds);

            fetch('/Search?' + params.toString(), {
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                signal: fetchController.signal
            })
                .then(function (res) { return res.ok ? res.text() : Promise.reject(new Error('HTTP ' + res.status)); })
                .then(function (html) {
                    Object.keys(markersBySlug).forEach(function (slug) {
                        markersBySlug[slug].pin.classList.remove('is-active');
                    });

                    resultsEl.innerHTML = html;
                    syncMarkers(readMarkers());
                    wireCards();
                })
                .catch(function (err) {
                    if (err.name !== 'AbortError') console.error('Search refresh failed', err);
                });
        }

        var initialItems = readMarkers();
        syncMarkers(initialItems);
        wireCards();

        var urlBounds = readUrlBounds();
        var itemsBounds = new google.maps.LatLngBounds();
        initialItems.forEach(function (item) {
            if (item.latitude != null && item.longitude != null) {
                itemsBounds.extend({ lat: item.latitude, lng: item.longitude });
            }
        });

        map.setOptions({ maxZoom: 15 });

        if (urlBounds) {
            map.fitBounds(urlBounds);
        } else if (!itemsBounds.isEmpty()) {
            map.fitBounds(itemsBounds);
        } else {
            map.setCenter({ lat: 48.4, lng: 31.2 });
            map.setZoom(6);
        }

        map.addListener('idle', function () {
            if (!initialMapFitDone) {
                initialMapFitDone = true;
                map.setOptions({ maxZoom: null });
                return;
            }

            var mapBounds = map.getBounds();
            if (!mapBounds) return;

            var ne = mapBounds.getNorthEast();
            var sw = mapBounds.getSouthWest();
            var bounds = { north: ne.lat(), south: sw.lat(), east: ne.lng(), west: sw.lng() };

            syncSearchBar(bounds);

            clearTimeout(refreshTimer);
            refreshTimer = setTimeout(function () { refreshResults(bounds); }, 300);
        });
    });
}
