import { createSearchField } from '../shared/search-field.js';

var STAR_ICON_SVG = await fetch('/icons/star.svg').then(function (res) { return res.text(); });

// PropertyCreateController.SearchAddress — пока заглушка с фиксированным ответом.
var SEARCH_URL = '/create-property/search-address';

var DETAIL_FIELD_IDS = ['Address', 'Street', 'District', 'CityId', 'CountryId'];

function fillAddressFields(item) {
    document.getElementById('Address').value = item.address;
    document.getElementById('Street').value = item.street;
    document.getElementById('District').value = item.district;
    document.getElementById('CityId').value = item.cityId;
    document.getElementById('CountryId').value = item.countryId;
    updateDetailsVisibility();
}

createSearchField({
    inputId: 'search',
    popoverId: 'search-popover',
    searchUrl: SEARCH_URL,
    iconFor: function () { return STAR_ICON_SVG; },
    onSelect: fillAddressFields,
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
