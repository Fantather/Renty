var inputs = document.querySelectorAll('.option-card__input');
var nextButton = document.querySelector('.property-category-next');

function updateSelection() {
    var anyChecked = false;

    inputs.forEach(function (input) {
        input.closest('.option-card').classList.toggle('is-selected', input.checked);
        if (input.checked) anyChecked = true;
    });

    nextButton.disabled = !anyChecked;
}

inputs.forEach(function (input) {
    input.addEventListener('change', updateSelection);
});

updateSelection();
