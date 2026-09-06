import { createModal } from '../shared/popover.js';

var amenitiesModal = createModal('allAmenitiesModal', 'closeAmenitiesModalBtn');

var showAllAmenitiesBtn = document.getElementById('showAllAmenitiesBtn');
if (showAllAmenitiesBtn) {
    showAllAmenitiesBtn.addEventListener('click', amenitiesModal.open);
}

var descriptionModal = createModal('descriptionModal', 'closeDescriptionModalBtn');

var showDescriptionBtn = document.getElementById('showDescriptionBtn');
if (showDescriptionBtn) {
    showDescriptionBtn.addEventListener('click', descriptionModal.open);
}
