import { createPopover } from '../shared/popover.js';
import { createGuestCounter } from '../shared/guest-counter.js';

var guestsBtn = document.getElementById('guestsBtn');
var guestsPopoverEl = document.getElementById('guestsPopover');

if (guestsBtn && guestsPopoverEl) {
    var guestsDismissible = createPopover('guestsBtn', 'guestsPopover', null);
    var guestsText = document.getElementById('guestsText');
    var guestsInput = document.getElementById('guestsInput');

    createGuestCounter({
        rows: guestsDismissible.root.querySelectorAll('.guests-popover__row'),
        initialCounts: { adult: 1 },
        onChange: function (counts) {
            var total = counts.adult + counts.child + counts.infant + counts.pet;
            guestsInput.value = total;
            guestsText.textContent = total + ' гостей';
        },
    });
}
