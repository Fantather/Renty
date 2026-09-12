import { limitCheckboxSelection } from '../shared/limited-checkboxes.js';

var checkboxes = Array.from(document.querySelectorAll('input[name="Highlights"]'));
limitCheckboxSelection(checkboxes, 2);
