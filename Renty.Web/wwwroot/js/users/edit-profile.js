import { createModal } from '../shared/popover.js';

var factModal = createModal('factModal', 'factModalClose');

var titleEl = document.getElementById('factModalTitle');
var hintEl = document.getElementById('factModalHint');
var inputEl = document.getElementById('factModalInput');
var counterEl = document.getElementById('factModalCounter');
var saveBtn = document.getElementById('factModalSave');

var currentValueInput = null;
var currentLabelEl = null;

function updateCounter() {
    var maxLength = Number(inputEl.getAttribute('maxlength'));
    var remaining = maxLength - inputEl.value.length;
    counterEl.textContent = 'Осталось ' + remaining + ' символов';
}

inputEl.addEventListener('input', updateCounter);

document.querySelectorAll('.edit-profile__fact-trigger').forEach(function (trigger) {
    trigger.addEventListener('click', function () {
        currentValueInput = document.getElementById(trigger.dataset.valueId);
        currentLabelEl = document.getElementById(trigger.dataset.labelId);

        titleEl.textContent = trigger.dataset.title;
        hintEl.textContent = trigger.dataset.hint;
        inputEl.setAttribute('maxlength', trigger.dataset.maxlength);
        inputEl.value = currentValueInput.value;
        updateCounter();

        factModal.open();
    });
});

saveBtn.addEventListener('click', function () {
    if (!currentValueInput) return;

    currentValueInput.value = inputEl.value;
    currentLabelEl.textContent = currentLabelEl.dataset.label
        ? currentLabelEl.dataset.label + ': ' + inputEl.value
        : inputEl.value;

    factModal.close();
});


// Модалка поколения
var generationModal = createModal('generationModal', 'generationModalClose');
var generationTrigger = document.getElementById('generationTrigger');
var generationToggle = document.getElementById('generationToggle');
var generationSaveBtn = document.getElementById('generationModalSave');
var showGenerationInput = document.getElementById('showGenerationInput');

generationTrigger.addEventListener('click', function () {
    generationToggle.checked = showGenerationInput.value.toLowerCase() === 'true';
    generationModal.open();
});

generationSaveBtn.addEventListener('click', function () {
    showGenerationInput.value = generationToggle.checked;
    generationTrigger.hidden = !generationToggle.checked;

    generationModal.close();
});

// Модалка языков
var languagesModal = createModal('languagesModal', 'languagesModalClose');
var languagesTrigger = document.getElementById('languagesTrigger');
var languagesRowLabel = document.getElementById('languagesRowLabel');
var languagesSearch = document.getElementById('languagesSearch');
var languagesSaveBtn = document.getElementById('languagesModalSave');
var languageIdsContainer = document.getElementById('languageIdsContainer');
var languageOptions = document.querySelectorAll('.edit-profile__option-row');

languagesTrigger.addEventListener('click', function () {
    languagesSearch.value = '';
    languageOptions.forEach(function (option) {
        option.hidden = false;
    });

    languagesModal.open();
});

languagesSearch.addEventListener('input', function () {
    var query = languagesSearch.value.toLowerCase();
    languageOptions.forEach(function (option) {
        option.hidden = !option.dataset.name.toLowerCase().startsWith(query);
    });
});

languagesSaveBtn.addEventListener('click', function () {
    var checkedBoxes = Array.from(document.querySelectorAll('.language-checkbox')).filter(function (checkbox) {
        return checkbox.checked;
    });

    languageIdsContainer.innerHTML = '';
    checkedBoxes.forEach(function (checkbox) {
        var hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = 'Input.LanguageIds';
        hiddenInput.value = checkbox.value;
        languageIdsContainer.appendChild(hiddenInput);
    });

    var names = checkedBoxes.map(function (checkbox) { return checkbox.dataset.name; });
    languagesRowLabel.textContent = 'Языки, на которых я говорю: ' + names.join(', ');

    languagesModal.close();
});

// Модалка города
var homeCityModal = createModal('homeCityModal', 'homeCityModalClose');
var homeCityTrigger = document.getElementById('homeCityTrigger');
var homeCityRowLabel = document.getElementById('homeCityRowLabel');
var homeCitySearch = document.getElementById('homeCitySearch');
var homeCityResults = document.getElementById('homeCityResults');
var homeCitySaveBtn = document.getElementById('homeCityModalSave');
var homeCityIdInput = document.getElementById('homeCityIdInput');
var homeCityDisplayInput = document.getElementById('homeCityDisplayInput');

// TODO: заглушка вместо CityController.SearchCity — когда понадобится реальный поиск,
// заменить mockCities на fetch('/City/search?searchTerm=' + encodeURIComponent(query)).
var mockCities = [
    { id: '11111111-1111-1111-1111-111111111111', name: 'Алмере', country: 'Нидерланды' },
    { id: '22222222-2222-2222-2222-222222222222', name: 'Алкмар', country: 'Нидерланды' },
    { id: '33333333-3333-3333-3333-333333333333', name: 'Алсмер', country: 'Нидерланды' },
    { id: '44444444-4444-4444-4444-444444444444', name: 'Одесса', country: 'Украина' },
    { id: '55555555-5555-5555-5555-555555555555', name: 'Киев', country: 'Украина' }
];

var selectedCity = null;

function buildCheckmark() {
    var check = document.createElement('span');
    check.className = 'edit-profile__city-check';
    return check;
}

function buildCityItem(city) {
    var item = document.createElement('button');
    item.type = 'button';
    item.className = 'edit-profile__option-row';

    var label = document.createElement('span');
    label.textContent = city.name + ', ' + city.country;
    item.appendChild(label);

    if (selectedCity && selectedCity.id === city.id) {
        item.appendChild(buildCheckmark());
    }

    item.addEventListener('click', function () {
        selectedCity = city;
        homeCitySaveBtn.disabled = false;

        homeCityResults.querySelectorAll('.edit-profile__city-check').forEach(function (check) {
            check.remove();
        });
        item.appendChild(buildCheckmark());
    });

    return item;
}

function renderCityResults(query) {
    homeCityResults.innerHTML = '';

    if (!query) return;

    var q = query.toLowerCase();
    mockCities
        .filter(function (city) { return city.name.toLowerCase().startsWith(q); })
        .forEach(function (city) {
            homeCityResults.appendChild(buildCityItem(city));
        });
}

homeCityTrigger.addEventListener('click', function () {
    selectedCity = null;
    homeCitySearch.value = '';
    homeCityResults.innerHTML = '';
    homeCitySaveBtn.disabled = true;

    homeCityModal.open();
});

homeCitySearch.addEventListener('input', function () {
    renderCityResults(homeCitySearch.value);
});

homeCitySaveBtn.addEventListener('click', function () {
    if (!selectedCity) return;

    var display = selectedCity.name + ', ' + selectedCity.country;

    homeCityIdInput.value = selectedCity.id;
    homeCityDisplayInput.value = display;
    homeCityRowLabel.textContent = 'Где я живу: ' + display;

    homeCityModal.close();
});
