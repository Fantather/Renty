import { createSearchField } from '../shared/search-field.js';
import { fetchPlaceSuggestions, endPlacesSession } from '../shared/places-autocomplete.js';

var DETAIL_FIELD_IDS = ['Address', 'Street', 'District', 'CityName', 'CountryName'];

// TODO: подключить, когда бэк добавит эндпоинт деталей места по placeId.
// Тогда selectSuggestion должен запросить детали (с тем же sessionToken), вызвать эту функцию
// с ответом и только после этого вызвать endPlacesSession().
function fillAddressFields(item) {
    document.getElementById('Address').value = item.address;
    document.getElementById('Street').value = item.street;
    document.getElementById('District').value = item.district;
    document.getElementById('CityName').value = item.cityName;
    document.getElementById('CountryName').value = item.countryName;
    updateDetailsVisibility();
}

function selectSuggestion(item) {
    document.getElementById('PlaceId').value = item.id;
    document.getElementById('Address').value = item.title;
    endPlacesSession();
    updateDetailsVisibility();
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
