import { loadGoogleMapsScript, createMap } from '../shared/google-maps.js';

var mapEl = document.getElementById('locationMap');
var canvasEl = document.getElementById('locationMapCanvas');
var latitudeInput = document.getElementById('latitudeInput');
var longitudeInput = document.getElementById('longitudeInput');

var position = { lat: parseFloat(mapEl.dataset.lat), lng: parseFloat(mapEl.dataset.lng) };

loadGoogleMapsScript(mapEl.dataset.apiKey).then(function () {
    var map = createMap(canvasEl, position);

    map.addListener('idle', function () {
        var center = map.getCenter();
        latitudeInput.value = String(center.lat()).replace('.', ',');
        longitudeInput.value = String(center.lng()).replace('.', ',');
    });
});
