import { limitCheckboxSelection } from '../shared/limited-checkboxes.js';

var checkboxes = Array.from(document.querySelectorAll('input[name="TagIds"]'));

limitCheckboxSelection(checkboxes, 2);

function updateSelection() {
    checkboxes.forEach(function (checkbox) {
        checkbox.closest('.option-card').classList.toggle('is-selected', checkbox.checked);
    });
}

checkboxes.forEach(function (checkbox) {
    checkbox.addEventListener('change', updateSelection);
});

updateSelection();
