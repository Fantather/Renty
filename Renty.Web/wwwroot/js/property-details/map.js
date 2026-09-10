import { loadGoogleMapsScript, createMap } from '../shared/google-maps.js';

var mapEl = document.getElementById('propertyMap');
var mapMarkerIconTemplate = document.getElementById('mapMarkerIconTemplate');

if (mapEl) {
    var position = { lat: parseFloat(mapEl.dataset.lat), lng: parseFloat(mapEl.dataset.lng) };
    loadGoogleMapsScript(mapEl.dataset.apiKey).then(function () {
        var map = createMap(mapEl, position);

        var pin = document.createElement('div');
        pin.className = 'map-marker';
        pin.appendChild(mapMarkerIconTemplate.content.cloneNode(true));

        new google.maps.marker.AdvancedMarkerElement({ position: position, map: map, content: pin, title: mapEl.dataset.name });
    });
}
