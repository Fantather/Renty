import { createSearchField } from '../shared/search-field.js';
import { fetchPlaceSuggestions, fetchPlaceDetails, endPlacesSession } from '../shared/places-autocomplete.js';

var FIELD_TO_DETAIL_KEY = {
    Address: 'streetName',
    Street: 'streetNumber',
    District: 'district',
    CityName: 'cityName',
    CountryName: 'countryName',
};

var DETAIL_FIELD_IDS = Object.keys(FIELD_TO_DETAIL_KEY);

function fillAddressFields(details) {
    DETAIL_FIELD_IDS.forEach(function (id) {
        document.getElementById(id).value = details[FIELD_TO_DETAIL_KEY[id]];
    });
    updateDetailsVisibility();
}

async function selectSuggestion(item) {
    document.getElementById('PlaceId').value = item.id;
    endPlacesSession();

    var addressDetails = await fetchPlaceDetails(item.id);
    fillAddressFields(addressDetails);
}

createSearchField({
    inputId: 'search',
    popoverId: 'search-popover',
    fetchItems: fetchPlaceSuggestions,
    onSelect: selectSuggestion,
});

var searchInput = document.getElementById('search');
var detailsBlock = document.getElementById('address-details');
var detailFields = DETAIL_FIELD_IDS.map(function (id) { return document.getElementById(id); });

function updateDetailsVisibility() {
    var searchHasValue = searchInput.value.trim() !== '';
    var anyFieldHasValue = detailFields.some(function (field) { return field.value.trim() !== ''; });

    detailsBlock.hidden = !searchHasValue && !anyFieldHasValue;
}

searchInput.addEventListener('input', updateDetailsVisibility);
detailFields.forEach(function (field) {
    field.addEventListener('input', updateDetailsVisibility);
});

updateDetailsVisibility();
