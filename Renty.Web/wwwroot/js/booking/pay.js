var root = document.getElementById('bookingPay');
var form = document.getElementById('bookingPayForm');
var submitBtn = document.getElementById('bookingPaySubmit');
var errorEl = document.getElementById('bookingPayError');

var stripe = Stripe(root.dataset.publishableKey, { locale: 'ru' });
var elementStyle = {
    base: {
        fontSize: '16px',
        color: '#2b2823',
        '::placeholder': { color: '#726c62' }
    }
};

var elements = stripe.elements();
var cardNumber = elements.create('cardNumber', { style: elementStyle, showIcon: true });
elements.create('cardExpiry', { style: elementStyle }).mount('#cardExpiryElement');
elements.create('cardCvc', { style: elementStyle }).mount('#cardCvcElement');
cardNumber.mount('#cardNumberElement');

function showError(message) {
    errorEl.textContent = message;
    errorEl.hidden = false;
}

form.addEventListener('submit', async function (e) {
    e.preventDefault();
    errorEl.hidden = true;
    submitBtn.disabled = true;

    var result = await stripe.confirmCardPayment(root.dataset.clientSecret, {
        payment_method: { card: cardNumber }
    });

    if (result.error) {
        showError(result.error.message);
        submitBtn.disabled = false;
        return;
    }

    var response = await fetch(root.dataset.completeUrl, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': form.querySelector('input[name="__RequestVerificationToken"]').value
        }
    });
    var data = await response.json().catch(function () { return {}; });

    if (!response.ok) {
        showError(data.errors ? data.errors.join(', ') : 'Не удалось подтвердить оплату');
        submitBtn.disabled = false;
        return;
    }

    window.location.href = data.redirectUrl;
});
