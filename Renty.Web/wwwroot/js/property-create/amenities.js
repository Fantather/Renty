var inputs = document.querySelectorAll('.option-card__input');

function updateSelection() {
    inputs.forEach(function (input) {
        input.closest('.option-card').classList.toggle('is-selected', input.checked);
    });
}

inputs.forEach(function (input) {
    input.addEventListener('change', updateSelection);
});

updateSelection();
