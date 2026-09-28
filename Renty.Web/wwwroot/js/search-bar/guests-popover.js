import { createPopover } from '../shared/popover.js';
import { createCounter } from '../shared/counter.js';

var guestsDismissible = createPopover('guestsSegment', 'guestsPopover', null);
var guestsPopover = guestsDismissible.root;
var guestsSegmentValue = document.getElementById('guestsSegmentValue');

var guestCountInput = document.getElementById('guestCountInput');
var petsInput = document.getElementById('petsInput');
var petsToggle = guestsPopover.querySelector('#petsToggle');

petsToggle.checked = petsInput.value === 'true';

createCounter({
    rows: guestsPopover.querySelectorAll('[data-counter]'),
    initialCounts: { guest: parseInt(guestCountInput.value, 10) || 0 },
    onChange: function (counts) {
        guestCountInput.value = counts.guest;
        guestsSegmentValue.textContent = counts.guest > 0 ? 'Гостей: ' + counts.guest : 'Кто едет?';
    },
});

petsToggle.addEventListener('change', function () {
    petsInput.value = petsToggle.checked ? 'true' : 'false';
});
