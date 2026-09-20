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

export function endPlacesSession() {
    sessionToken = null;
}
