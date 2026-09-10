import { createDateRangePicker } from '../shared/date-range-picker.js';
import { formatIsoDate } from '../shared/date-utils.js';

function formatDisplayDate(date) {
    var day = String(date.getDate()).padStart(2, '0');
    var month = String(date.getMonth() + 1).padStart(2, '0');
    return day + '.' + month + '.' + date.getFullYear();
}

var calendarSection = document.getElementById('calendar');
var calendarMonthSlots = document.querySelectorAll('#calendar [data-month-slot]');
var calendarPrevBtn = document.getElementById('calendarPrevBtn');
var calendarNextBtn = document.getElementById('calendarNextBtn');

if (calendarMonthSlots.length && calendarPrevBtn && calendarNextBtn) {
    var checkInDateInput = document.getElementById('checkInDateInput');
    var checkOutDateInput = document.getElementById('checkOutDateInput');
    var checkInDateText = document.getElementById('checkInDateText');
    var checkOutDateText = document.getElementById('checkOutDateText');
    var checkInDateBtn = document.getElementById('checkInDateBtn');
    var checkOutDateBtn = document.getElementById('checkOutDateBtn');
    var totalPriceEl = document.getElementById('totalPrice');
    var propertySlug = document.getElementById('propertyDetails').dataset.slug;

    checkInDateBtn.addEventListener('click', function () {
        calendarSection.scrollIntoView({ behavior: 'smooth' });
    });
    checkOutDateBtn.addEventListener('click', function () {
        calendarSection.scrollIntoView({ behavior: 'smooth' });
    });

    async function updateTotalPrice(checkIn, checkOut) {
        var url = '/Properties/Price?slug=' + encodeURIComponent(propertySlug) +
            '&checkIn=' + formatIsoDate(checkIn) + '&checkOut=' + formatIsoDate(checkOut);
        var res = await fetch(url);

        if (!res.ok) {
            totalPriceEl.hidden = true;
            return;
        }

        var data = await res.json();
        totalPriceEl.textContent = data.nights + ' ночей · $' + data.total;
        totalPriceEl.hidden = false;
    }

    var bookingPicker = createDateRangePicker({
        monthSlots: calendarMonthSlots,
        prevBtn: calendarPrevBtn,
        nextBtn: calendarNextBtn,
        onSelect: function (checkIn, checkOut) {
            checkInDateInput.value = checkIn ? formatIsoDate(checkIn) : '';
            checkOutDateInput.value = checkOut ? formatIsoDate(checkOut) : '';
            checkInDateText.textContent = checkIn ? formatDisplayDate(checkIn) : 'Добавить дату';
            checkOutDateText.textContent = checkOut ? formatDisplayDate(checkOut) : 'Добавить дату';

            if (checkIn && checkOut) {
                updateTotalPrice(checkIn, checkOut);
            } else {
                totalPriceEl.hidden = true;
            }
        },
    });

    bookingPicker.render();
}
