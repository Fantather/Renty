var inputs = document.querySelectorAll('.booking-option__input');

function updateSelection() {
    inputs.forEach(function (input) {
        input.closest('.booking-option').classList.toggle('is-selected', input.checked);
    });
}

inputs.forEach(function (input) {
    input.addEventListener('change', updateSelection);
});

updateSelection();
