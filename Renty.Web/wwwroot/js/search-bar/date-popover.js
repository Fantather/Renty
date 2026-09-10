import { createPopover } from '../shared/popover.js';
import { createDateRangePicker } from '../shared/date-range-picker.js';
import { formatShortDate, formatIsoDate } from '../shared/date-utils.js';

var dateSegmentValue = document.getElementById('dateSegmentValue');
var checkInInput = document.getElementById('checkInInput');
var checkOutInput = document.getElementById('checkOutInput');

var picker = createDateRangePicker({
    monthSlots: document.querySelectorAll('[data-month-slot]'),
    prevBtn: document.getElementById('prevMonthBtn'),
    nextBtn: document.getElementById('nextMonthBtn'),
    onSelect: function (checkIn, checkOut) {
        checkInInput.value = checkIn ? formatIsoDate(checkIn) : '';
        checkOutInput.value = checkOut ? formatIsoDate(checkOut) : '';

        if (checkIn && checkOut) {
            dateSegmentValue.textContent = formatShortDate(checkIn) + ' – ' + formatShortDate(checkOut);
        } else if (checkIn) {
            dateSegmentValue.textContent = formatShortDate(checkIn) + ' – ?';
        } else {
            dateSegmentValue.textContent = 'Когда?';
        }
    },
});

createPopover('dateSegment', 'datePopover', picker.render);
