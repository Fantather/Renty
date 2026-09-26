var sessionToken = null;

export async function fetchPlaceSuggestions(query) {
    if (!sessionToken) sessionToken = crypto.randomUUID();

    var res = await fetch('/api/places/autocomplete?input=' + encodeURIComponent(query)
        + '&sessionToken=' + sessionToken);
    if (!res.ok) return [];

    var suggestions = await res.json();
    return suggestions.map(function (s) {
        return { id: s.placeId, title: s.text };
    });
}

export async function fetchCitySuggestions(query) {
    if (!sessionToken) sessionToken = crypto.randomUUID();

    var res = await fetch('/api/places/autocomplete-city?input=' + encodeURIComponent(query)
        + '&sessionToken=' + sessionToken);
    if (!res.ok) return [];

    var suggestions = await res.json();
    return suggestions.map(function (s) {
        return { id: s.placeId, name: s.cityName, country: s.countryName };
    });
}

export async function resolveCity(placeId) {
    var res = await fetch('/api/places/resolve-city?placeId=' + encodeURIComponent(placeId));
    if (!res.ok) return null;

    return await res.json();
}

export async function fetchPlaceDetails(placeId) {
    var res = await fetch('/api/places/details?placeId=' + encodeURIComponent(placeId));
    if (!res.ok) return {};

    var address = await res.json();
    return address;
}

export function endPlacesSession() {
    sessionToken = null;
}
