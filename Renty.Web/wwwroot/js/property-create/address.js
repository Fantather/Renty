import { createSearchField } from '../shared/search-field.js';

var STAR_ICON_SVG = await fetch('/icons/star.svg').then(function (res) { return res.text(); });

// PropertyCreateController.SearchAddress — пока заглушка с фиксированным ответом.
var SEARCH_URL = '/create-property/search-address';

function fillAddressFields(item) {
    document.getElementById('Address').value = item.address;
    document.getElementById('Street').value = item.street;
    document.getElementById('District').value = item.district;
    document.getElementById('CityId').value = item.cityId;
    document.getElementById('CountryId').value = item.countryId;
}

createSearchField({
    inputId: 'search',
    popoverId: 'search-popover',
    searchUrl: SEARCH_URL,
    iconFor: function () { return STAR_ICON_SVG; },
    onSelect: fillAddressFields,
});
