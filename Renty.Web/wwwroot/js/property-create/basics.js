import { createCounter } from '../shared/counter.js';

var countInputs = {
    maxGuests: document.getElementById('MaxGuests'),
    bedrooms: document.getElementById('BedroomsCount'),
    beds: document.getElementById('BedsCount'),
    bathrooms: document.getElementById('BathroomsCount'),
};

var initialCounts = {
    maxGuests: parseInt(countInputs.maxGuests.value, 10) || 1,
    bedrooms: parseInt(countInputs.bedrooms.value, 10) || 0,
    beds: parseInt(countInputs.beds.value, 10) || 0,
    bathrooms: parseInt(countInputs.bathrooms.value, 10) || 0,
};

function syncInputs(counts) {
    countInputs.maxGuests.value = counts.maxGuests;
    countInputs.bedrooms.value = counts.bedrooms;
    countInputs.beds.value = counts.beds;
    countInputs.bathrooms.value = counts.bathrooms;
}

syncInputs(initialCounts);

createCounter({
    rows: document.querySelectorAll('.property-basics-counters .counter-row'),
    initialCounts: initialCounts,
    onChange: syncInputs,
});
