var MAX_PERCENT = 99;

var percentInputs = document.querySelectorAll('.discount-card__percent-input');

percentInputs.forEach(function (input) {
    input.addEventListener('input', function () {
        if (input.value === '') return;

        var value = Number(input.value);
        if (value > MAX_PERCENT) input.value = MAX_PERCENT;
        if (value < 0) input.value = 0;

        if (Number(input.value) === 0) {
            input.closest('.discount-card').querySelector('.discount-toggle__input').checked = false;
        }
    });
});

var weeklyInput = document.getElementById('WeeklyDiscountPercent');
var monthlyInput = document.getElementById('MonthlyDiscountPercent');
var monthlyCard = monthlyInput.closest('.discount-card');
var monthlyError = document.getElementById('monthlyDiscountError');
var submitButton = document.querySelector('button[type="submit"][form="' + monthlyInput.form.id + '"]');

function validateMonthly() {
    var weekly = Number(weeklyInput.value) || 0;
    var monthly = Number(monthlyInput.value) || 0;
    var isInvalid = monthly <= weekly;

    monthlyCard.classList.toggle('discount-card--error', isInvalid);
    monthlyError.hidden = !isInvalid;
    monthlyError.textContent = isInvalid
        ? 'Скидка за месяц должна быть больше скидки за неделю, которая составляет ' + weekly + '%'
        : '';
    submitButton.disabled = isInvalid;
}

weeklyInput.addEventListener('input', validateMonthly);
monthlyInput.addEventListener('input', validateMonthly);

validateMonthly();
