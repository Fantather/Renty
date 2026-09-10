import { createPopover } from '../shared/popover.js';
import { createGuestCounter } from '../shared/guest-counter.js';

var guestsDismissible = createPopover('guestsSegment', 'guestsPopover', null);
var guestsPopover = guestsDismissible.root;
var guestsSegmentValue = document.getElementById('guestsSegmentValue');

var countInputs = {
    adult: document.getElementById('adultCountInput'),
    child: document.getElementById('childCountInput'),
    infant: document.getElementById('infantCountInput'),
    pet: document.getElementById('petCountInput'),
};

var initialCounts = {
    adult: parseInt(countInputs.adult.value, 10) || 0,
    child: parseInt(countInputs.child.value, 10) || 0,
    infant: parseInt(countInputs.infant.value, 10) || 0,
    pet: parseInt(countInputs.pet.value, 10) || 0,
};

createGuestCounter({
    rows: guestsPopover.querySelectorAll('.guests-popover__row'),
    initialCounts: initialCounts,
    onChange: function (counts) {
        countInputs.adult.value = counts.adult;
        countInputs.child.value = counts.child;
        countInputs.infant.value = counts.infant;
        countInputs.pet.value = counts.pet;

        var total = counts.adult + counts.child + counts.infant + counts.pet;
        guestsSegmentValue.textContent = total > 0 ? 'Гостей: ' + total : 'Кто едет?';
    },
});
