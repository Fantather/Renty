import { loginModal } from '../login-modal/login-modal.js';

document.addEventListener('click', function (e) {
    var button = e.target.closest('.property-card__favorite');
    if (!button) return;

    e.preventDefault();

    var card = button.closest('.property-card');
    var slug = card.dataset.propertySlug;

    button.disabled = true;

    fetch('/Favorites/ToggleFavorite', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(slug)
    })
        .then(function (response) {
            if (response.status === 401) {
                loginModal.open();
                return null;
            }
            return response.json();
        })
        .then(function (data) {
            if (!data || data.success !== true) return;

            document.querySelectorAll('.property-card[data-property-slug="' + slug + '"] .property-card__favorite')
                .forEach(function (btn) {
                    btn.classList.toggle('is-active', data.isFavorite);
                });
        })
        .finally(function () {
            button.disabled = false;
        });
});
