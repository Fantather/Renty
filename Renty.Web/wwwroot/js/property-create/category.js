var inputs = document.querySelectorAll('.option-card__input');
var nextButton = document.querySelector('button[type="submit"][form="' + inputs[0].form.id + '"]:not([name="saveAndExit"])');

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
