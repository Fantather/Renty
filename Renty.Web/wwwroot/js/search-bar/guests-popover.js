import { createPopover } from '../shared/popover.js';

// Who's coming
var guestsDismissible = createPopover('guestsSegment', 'guestsPopover', null);
var guestsPopover = guestsDismissible.root;
var guestsSegmentValue = document.getElementById('guestsSegmentValue');

var counterRows = guestsPopover.querySelectorAll('.guests-popover__row');

var countInputs = {
    adult: document.getElementById('adultCountInput'),
    child: document.getElementById('childCountInput'),
    infant: document.getElementById('infantCountInput'),
    pet: document.getElementById('petCountInput')
};

var counts = {
    adult: parseInt(countInputs.adult.value, 10) || 0,
    child: parseInt(countInputs.child.value, 10) || 0,
    infant: parseInt(countInputs.infant.value, 10) || 0,
    pet: parseInt(countInputs.pet.value, 10) || 0
};

counterRows.forEach(function (row) {
    var key = row.dataset.counter;
    var countEl = row.querySelector('.guests-popover__count');
    var minusBtn = row.querySelector('.guests-popover__btn--minus');
    var plusBtn = row.querySelector('.guests-popover__btn--plus');

    renderCounter(key, countEl, minusBtn);

    minusBtn.addEventListener('click', function () {
        counts[key] = Math.max(0, counts[key] - 1);
        countInputs[key].value = counts[key];
        renderCounter(key, countEl, minusBtn);
        updateGuestsLabel();
    });

    plusBtn.addEventListener('click', function () {
        counts[key]++;
        countInputs[key].value = counts[key];
        renderCounter(key, countEl, minusBtn);
        updateGuestsLabel();
    });
});

function renderCounter(key, countEl, minusBtn) {
    countEl.textContent = counts[key];
    minusBtn.disabled = counts[key] === 0;
}

function updateGuestsLabel() {
    var total = counts.adult + counts.child + counts.infant + counts.pet;
    guestsSegmentValue.textContent = total > 0 ? 'Гостей: ' + total : 'Кто едет?';
}
