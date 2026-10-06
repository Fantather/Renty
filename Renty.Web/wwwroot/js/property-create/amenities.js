var inputs = document.querySelectorAll('.option-card__input');

function updateCard(input) {
    input.closest('.option-card').classList.toggle('is-selected', input.checked);
}

inputs.forEach(function (input) {
    input.addEventListener('change', function () {
        updateCard(input);
    });
    updateCard(input);
});
