var googleMapsPromise = null;

export function loadGoogleMapsScript(apiKey) {
    if (googleMapsPromise) return googleMapsPromise;

    googleMapsPromise = new Promise(function (resolve) {
        window.__onGoogleMapsLoaded = resolve;
        var script = document.createElement('script');
        script.src = 'https://maps.googleapis.com/maps/api/js?key=' + apiKey + '&libraries=marker&callback=__onGoogleMapsLoaded';
        script.async = true;
        document.head.appendChild(script);
    });

    return googleMapsPromise;
}

export function createMap(container, center, options) {
    return new google.maps.Map(container, Object.assign({
        center: center,
        zoom: 14,
        disableDefaultUI: true,
        zoomControl: true,
        fullscreenControl: true,
        streetViewControl: true,
        gestureHandling: 'greedy',
        scrollwheel: false,
        mapId: 'DEMO_MAP_ID',
    }, options));
}
