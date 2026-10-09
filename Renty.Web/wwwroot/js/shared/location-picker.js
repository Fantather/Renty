import { loadGoogleMapsScript, createMap, createApproximateCircle } from './google-maps.js';

var LOCKED_OPTIONS = { gestureHandling: 'none', zoomControl: false, keyboardShortcuts: false };
var UNLOCKED_OPTIONS = { gestureHandling: 'greedy', zoomControl: true, keyboardShortcuts: true };

export function createLocationPicker(options) {
    var locked = !!(options && options.locked);

    var mapEl = document.getElementById('locationMap');
    var canvasEl = document.getElementById('locationMapCanvas');
    var latitudeInput = document.getElementById('latitudeInput');
    var longitudeInput = document.getElementById('longitudeInput');

    var initialPosition = { lat: parseFloat(mapEl.dataset.lat), lng: parseFloat(mapEl.dataset.lng) };
    var map = null;

    function setInputs(position) {
        latitudeInput.value = String(position.lat).replace('.', ',');
        longitudeInput.value = String(position.lng).replace('.', ',');
    }

    function setLocked(value) {
        locked = value;
        mapEl.classList.toggle('location-map--locked', locked);
        if (map) map.setOptions(locked ? LOCKED_OPTIONS : UNLOCKED_OPTIONS);
    }

    setLocked(locked);

    loadGoogleMapsScript(mapEl.dataset.apiKey).then(function () {
        map = createMap(canvasEl, initialPosition, Object.assign({}, locked ? LOCKED_OPTIONS : UNLOCKED_OPTIONS, options && options.mapOptions));

        if (options && options.approximate) {
            var circle = createApproximateCircle({ map: map, center: initialPosition });
            map.addListener('center_changed', function () {
                circle.setCenter(map.getCenter());
            });
        }

        map.addListener('idle', function () {
            var center = map.getCenter();
            setInputs({ lat: center.lat(), lng: center.lng() });
        });
    });

    return {
        unlock: function () { setLocked(false); },
        reset: function () {
            if (map) map.setCenter(initialPosition);
            setInputs(initialPosition);
            setLocked(true);
        },
    };
}
