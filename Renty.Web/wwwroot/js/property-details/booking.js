import { createDateRangePicker } from '../shared/date-range-picker.js';
import { formatIsoDate } from '../shared/date-utils.js';
import { loginModal } from '../login-modal/login-modal.js';

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

    var bookedRanges = JSON.parse(calendarSection.dataset.bookedRanges || '[]').map(function (range) {
        return { from: parseIsoDate(range[0]), to: parseIsoDate(range[1]) };
    });

    var today = new Date();
    today.setHours(0, 0, 0, 0);

    function parseIsoDate(value) {
        var parts = value.split('-');
        return new Date(Number(parts[0]), Number(parts[1]) - 1, Number(parts[2]));
    }

    function isBookedNight(date) {
        return bookedRanges.some(function (range) {
            return date >= range.from && date < range.to;
        });
    }

    function overlapsBooking(checkIn, checkOut) {
        return bookedRanges.some(function (range) {
            return range.from < checkOut && range.to > checkIn;
        });
    }

    function isDayDisabled(date, pendingCheckIn) {
        if (date < today) return true;
        if (pendingCheckIn && date > pendingCheckIn) return overlapsBooking(pendingCheckIn, date);
        return isBookedNight(date);
    }

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
        isDisabled: isDayDisabled,
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

var bookingSubmitBtn = document.getElementById('bookingSubmitBtn');
var bookingErrorEl = document.getElementById('bookingError');

function showBookingError(message) {
    bookingErrorEl.textContent = message;
    bookingErrorEl.hidden = false;
}

bookingSubmitBtn.addEventListener('click', function () {
    var checkIn = document.getElementById('checkInDateInput').value;
    var checkOut = document.getElementById('checkOutDateInput').value;

    bookingErrorEl.hidden = true;

    if (!checkIn || !checkOut) {
        showBookingError('Выберите даты заезда и выезда');
        document.getElementById('calendar').scrollIntoView({ behavior: 'smooth' });
        return;
    }

    bookingSubmitBtn.disabled = true;

    fetch('/Booking/Submit', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({
            propertyId: document.getElementById('propertyDetails').dataset.propertyId,
            checkIn: checkIn,
            checkOut: checkOut,
            guests: Number(document.getElementById('guestsInput').value),
            paymentMethod: 2
        })
    })
        .then(function (response) {
            if (response.status === 401) {
                loginModal.open();
                return null;
            }
            return response.json().then(function (data) {
                if (!response.ok) {
                    showBookingError(data.errors ? data.errors.join(', ') : 'Не удалось забронировать');
                    return null;
                }
                window.location.href = '/Booking/Result?bookingId=' + data.bookingId;
            });
        })
        .catch(function () {
            showBookingError('Не удалось забронировать');
        })
        .finally(function () {
            bookingSubmitBtn.disabled = false;
        });
});
