var inputs = document.querySelectorAll('.option-card__input');

inputs.forEach(function (input) {
    input.addEventListener('change', function () {
        input.closest('.option-card').classList.toggle('is-selected', input.checked);
    });
});
