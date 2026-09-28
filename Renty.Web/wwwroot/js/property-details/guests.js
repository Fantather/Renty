import { createPopover } from '../shared/popover.js';
import { createCounter } from '../shared/counter.js';

var guestsBtn = document.getElementById('guestsBtn');
var guestsPopoverEl = document.getElementById('guestsPopover');

if (guestsBtn && guestsPopoverEl) {
    var guestsDismissible = createPopover('guestsBtn', 'guestsPopover', null);
    var guestsText = document.getElementById('guestsText');
    var guestsInput = document.getElementById('guestsInput');
    var petsInput = document.getElementById('guestsPetsInput');
    var petsToggle = guestsDismissible.root.querySelector('#petsToggle');

    petsToggle.checked = petsInput.value === 'true';

    createCounter({
        rows: guestsDismissible.root.querySelectorAll('[data-counter]'),
        initialCounts: { guest: parseInt(guestsInput.value, 10) || 1 },
        onChange: function (counts) {
            guestsInput.value = counts.guest;
            guestsText.textContent = counts.guest + ' гостей';
        },
    });

    petsToggle.addEventListener('change', function () {
        petsInput.value = petsToggle.checked ? 'true' : 'false';
    });
}
