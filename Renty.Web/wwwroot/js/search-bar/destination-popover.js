import { createSearchField } from '../shared/search-field.js';
import { fetchPlaceSuggestions, endPlacesSession } from '../shared/places-autocomplete.js';

var destinationInput = document.getElementById('destinationInput');

createSearchField({
    inputId: 'destinationInput',
    popoverId: 'destinationPopover',
    fetchItems: fetchPlaceSuggestions,
    onSelect: function (item) {
        destinationInput.value = item.title;
        endPlacesSession();
    },
});
