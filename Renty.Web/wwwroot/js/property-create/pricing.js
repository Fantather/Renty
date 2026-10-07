var priceInput = document.getElementById('PricePerNight');
var percentInput = document.getElementById('WeekendPricePercent');
var hint = document.getElementById('weekendPriceHint');
var currency = priceInput.form.dataset.currency;

function updateHint() {
    var price = Number(priceInput.value) || 0;
    var percent = Number(percentInput.value) || 0;

    if (price <= 0 || percent <= 0) {
        hint.textContent = '';
        return;
    }

    var weekendPrice = Math.round(price * (1 + percent / 100));
    hint.textContent = currency + ' ' + weekendPrice + ' на пятницу и субботу';
}

priceInput.addEventListener('input', updateHint);
percentInput.addEventListener('input', updateHint);

updateHint();
