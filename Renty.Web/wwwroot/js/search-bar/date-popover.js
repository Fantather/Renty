import { createPopover } from '../shared/popover.js';
import { monthNames, formatShortDate, formatIsoDate } from '../shared/date-utils.js';

// Popover
var dateDismissible = createPopover('dateSegment', 'datePopover', renderCalendars);
var datePopover = dateDismissible.root;

// Calendars
var dateSegmentValue = document.getElementById('dateSegmentValue');
var checkInInput = document.getElementById('checkInInput');
var checkOutInput = document.getElementById('checkOutInput');

var monthSlots = document.querySelectorAll('[data-month-slot]');
var prevMonthBtn = document.getElementById('prevMonthBtn');
var nextMonthBtn = document.getElementById('nextMonthBtn');
var weekdayLabels = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'];

var viewDate = new Date();
viewDate.setDate(1);
var checkIn = null;
var checkOut = null;


prevMonthBtn.addEventListener('click', function () {
    viewDate.setMonth(viewDate.getMonth() - 1);
    renderCalendars();
});

nextMonthBtn.addEventListener('click', function () {
    viewDate.setMonth(viewDate.getMonth() + 1);
    renderCalendars();
});


// Сalendar
function renderCalendars() {
    for (var i = 0; i < monthSlots.length; i++) {
        var monthDate = new Date(viewDate.getFullYear(), viewDate.getMonth() + i, 1);
        monthSlots[i].innerHTML = '';
        monthSlots[i].appendChild(buildMonth(monthDate.getFullYear(), monthDate.getMonth()));
    }
}

function buildMonth(year, month) {
    var wrapper = document.createElement('div');

    var title = document.createElement('div');
    title.className = 'date-popover__month-title';
    title.textContent = monthNames[month] + ' ' + year;
    wrapper.appendChild(title);

    var grid = document.createElement('div');
    grid.className = ('date-popover__grid');

    weekdayLabels.forEach(function (label) {
        var cell = document.createElement('div');
        cell.className = 'date-popover__weekday';
        cell.textContent = label;
        grid.appendChild(cell);
    });

    var firstDay = new Date(year, month, 1);
    var leadingBlanks = (firstDay.getDay() + 6) % 7;
    for (var b = 0; b < leadingBlanks; b++) {
        grid.appendChild(document.createElement('div'));
    }

    var daysInMonth = new Date(year, month + 1, 0).getDate();
    for (var d = 1; d <= daysInMonth; d++) {
        grid.appendChild(buildDayCell(year, month, d));
    }

    wrapper.appendChild(grid);
    return wrapper;
}

function buildDayCell(year, month, day) {
    var cellDate = new Date(year, month, day);

    var dayBtn = document.createElement('button');
    dayBtn.type = 'button';
    dayBtn.className = 'date-popover__day';
    dayBtn.textContent = day;

    var isRangeStart = sameDay(cellDate, checkIn);
    var isRangeEnd = sameDay(cellDate, checkOut);
    var isInRange = checkIn && checkOut && cellDate > checkIn && cellDate < checkOut;

    if (isRangeStart || isRangeEnd) dayBtn.classList.add('is-selected');
    if (isRangeStart && checkOut) dayBtn.classList.add('is-range-start');
    if (isRangeEnd) dayBtn.classList.add('is-range-end');
    if (isInRange) dayBtn.classList.add('is-in-range');

    dayBtn.addEventListener('click', function () {
        selectDate(cellDate);
    });

    return dayBtn;
}

function sameDay(a, b) {
    return !!a && !!b && a.getTime() === b.getTime();
}

function selectDate(date) {
    if (!checkIn || checkOut || date <= checkIn) {
        checkIn = date;
        checkOut = null;
    } else {
        checkOut = date;
    }

    checkInInput.value = checkIn ? formatIsoDate(checkIn) : '';
    checkOutInput.value = checkOut ? formatIsoDate(checkOut) : '';

    updateSegmentLabel();
    renderCalendars();
}

function updateSegmentLabel() {
    if (checkIn && checkOut) {
        dateSegmentValue.textContent = formatShortDate(checkIn) + ' – ' + formatShortDate(checkOut);
    } else if (checkIn) {
        dateSegmentValue.textContent = formatShortDate(checkIn) + ' – ?';
    } else {
        dateSegmentValue.textContent = 'Когда?';
    }
}
