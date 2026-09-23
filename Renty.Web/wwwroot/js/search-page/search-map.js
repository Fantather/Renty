import { loadGoogleMapsScript, createMap } from '../shared/google-maps.js';

var mapEl = document.getElementById('searchMap');
var canvasEl = document.getElementById('searchMapCanvas');
var mapDataEl = document.getElementById('searchMapData');

if (mapEl && canvasEl && mapDataEl) {
    var markers = JSON.parse(mapDataEl.textContent);

    loadGoogleMapsScript(mapEl.dataset.apiKey).then(function () {
        var map = createMap(canvasEl, { lat: 0, lng: 0 }, { scrollwheel: true });
        var bounds = new google.maps.LatLngBounds();

        var infoWindow = new google.maps.InfoWindow();
        markers.forEach(function (marker) {
            if (marker.latitude == null || marker.longitude == null) return;

            var position = { lat: marker.latitude, lng: marker.longitude };
            bounds.extend(position);

            var pin = document.createElement('div');
            pin.className = 'map-price-pin';
            pin.textContent = '$' + Math.round(marker.pricePerNight);

            var advancedMarker = new google.maps.marker.AdvancedMarkerElement({
                position: position,
                map: map,
                content: pin
            });

            var card = document.querySelector('.property-card[data-property-id="' + marker.slug + '"]');
            if (card) {
                card.addEventListener('mouseenter', function () { pin.classList.add('is-active'); });
                card.addEventListener('mouseleave', function () { pin.classList.remove('is-active'); });
            }

            pin.addEventListener('click', function () {
                if (card) {
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
                            infoWindow.close();
                        });
                        imageWrap.appendChild(closeBtn);
                    }

                    infoWindow.setContent(popupCard);
                    infoWindow.open({ map: map, anchor: advancedMarker })
                }
            });
        });

        map.fitBounds(bounds);

        var idleFired = false;
        var boundsDebounceTimer = null;

        map.addListener('idle', function () {
            if (!idleFired) {
                idleFired = true;
                return;
            }

            clearTimeout(boundsDebounceTimer);
            boundsDebounceTimer = setTimeout(function () {
                var mapBounds = map.getBounds();
                if (!mapBounds) return;

                var ne = mapBounds.getNorthEast();
                var sw = mapBounds.getSouthWest();

                var params = new URLSearchParams({
                    north: ne.lat(),
                    south: sw.lat(),
                    east: ne.lng(),
                    west: sw.lng()
                });

                fetch('/Search/PropertiesInBounds?' + params.toString())
                    .then(function (res) { return res.json(); })
                    .then(function (data) { console.log('Properties in bounds:', data); });
            }, 300);
        });
    });
}
