import { loadGoogleMapsScript, createMap } from '../shared/google-maps.js';

var mapEl = document.getElementById('locationPreviewMap');
var canvasEl = document.getElementById('locationPreviewCanvas');
var pinEl = document.querySelector('.location-preview__pin');
var toggleInput = document.getElementById('showExactLocationInput');

var position = { lat: parseFloat(mapEl.dataset.lat), lng: parseFloat(mapEl.dataset.lng) };

loadGoogleMapsScript(mapEl.dataset.apiKey).then(function () {
    var map = createMap(canvasEl, position, {
        gestureHandling: 'none',
        draggable: false,
        scrollwheel: false,
        disableDefaultUI: true,
        zoomControl: false,
        fullscreenControl: false,
        streetViewControl: false,
    });

    var TARGET_RADIUS = 300;

    var circle = new google.maps.Circle({
        center: position,
        radius: 0,
        fillColor: '#000',
        fillOpacity: 0.15,
        strokeColor: '#000',
        strokeOpacity: 0.3,
        strokeWeight: 1,
    });

    function animateCircleIn() {
        var start = performance.now();
        var duration = 400;

        function step(now) {
            var progress = Math.min((now - start) / duration, 1);
            var eased = 1 - Math.pow(1 - progress, 3);
            circle.setRadius(TARGET_RADIUS * eased);

            if (progress < 1) {
                requestAnimationFrame(step);
            }
        }

        requestAnimationFrame(step);
    }

    function updateVisibility() {
        var showExact = toggleInput.checked;
        pinEl.hidden = !showExact;

        if (showExact) {
            circle.setMap(null);
        } else {
            circle.setRadius(0);
            circle.setMap(map);
            animateCircleIn();
        }
    }

    toggleInput.addEventListener('change', updateVisibility);
    updateVisibility();
});
