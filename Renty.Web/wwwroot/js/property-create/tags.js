import { limitCheckboxSelection } from '../shared/limited-checkboxes.js';

var checkboxes = Array.from(document.querySelectorAll('input[name="TagIds"]'));
var nextButton = document.querySelector('button[type="submit"][form="' + checkboxes[0].form.id + '"]');

limitCheckboxSelection(checkboxes, 2);

function updateSelection() {
    checkboxes.forEach(function (checkbox) {
        checkbox.closest('.option-card').classList.toggle('is-selected', checkbox.checked);
    });
    nextButton.disabled = !checkboxes.some(function (checkbox) { return checkbox.checked; });
}

checkboxes.forEach(function (checkbox) {
    checkbox.addEventListener('change', updateSelection);
});

updateSelection();
