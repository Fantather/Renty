import { createModal } from '../shared/popover.js';
import { submitFormAsync } from '../shared/ajax-form.js';
import { createLocationPicker } from '../shared/location-picker.js';

var editor = document.getElementById('location-editor');
var picker = createLocationPicker({
    locked: true,
    approximate: editor.dataset.approximate === 'true',
    mapOptions: { fullscreenControl: false },
});
var adjustButton = document.getElementById('location-adjust');
var cancelButton = document.getElementById('location-cancel');
var saveButton = document.getElementById('location-save');
var mapErrors = editor.querySelector('.location-editor__errors');

function setAdjusting(adjusting) {
    adjustButton.hidden = adjusting;
    cancelButton.hidden = !adjusting;
    saveButton.hidden = !adjusting;
}

adjustButton.addEventListener('click', function () {
    picker.unlock();
    setAdjusting(true);
});

cancelButton.addEventListener('click', function () {
    picker.reset();
    mapErrors.innerHTML = '';
    setAdjusting(false);
});

submitFormAsync(document.getElementById('location-form'), editor.dataset.submitUrl, mapErrors);

document.querySelectorAll('[data-panel]').forEach(function (row) {
    var panelId = row.dataset.panel;
    var panel = createModal(panelId, panelId + '-close');

    row.addEventListener('click', panel.open);

    submitFormAsync(
        panel.root.querySelector('form'),
        panel.root.dataset.submitUrl,
        panel.root.querySelector('.side-panel__errors'));
});
