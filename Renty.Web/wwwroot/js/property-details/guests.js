import { createPopover } from '../shared/popover.js';
import { createCounter } from '../shared/counter.js';

var guestsBtn = document.getElementById('guestsBtn');
var guestsPopoverEl = document.getElementById('guestsPopover');

if (guestsBtn && guestsPopoverEl) {
    var guestsDismissible = createPopover('guestsBtn', 'guestsPopover', null);
    var guestsText = document.getElementById('guestsText');
    var guestsInput = document.getElementById('guestsInput');

    createCounter({
        rows: guestsDismissible.root.querySelectorAll('.counter-row'),
        initialCounts: { adult: 1 },
        onChange: function (counts) {
            var total = counts.adult + counts.child + counts.infant + counts.pet;
            guestsInput.value = total;
            guestsText.textContent = total + ' гостей';
        },
    });
}
