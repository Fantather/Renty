import { createModal } from '../shared/popover.js';
import { fetchCitySuggestions, resolveCity, endPlacesSession } from '../shared/places-autocomplete.js';

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

if (generationTrigger) {
    generationTrigger.addEventListener('click', function () {
        generationToggle.checked = showGenerationInput.value.toLowerCase() === 'true';
        generationModal.open();
    });
}

generationSaveBtn.addEventListener('click', function () {
    showGenerationInput.value = generationToggle.checked;
    if (generationTrigger) generationTrigger.hidden = !generationToggle.checked;

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

var selectedCity = null;
var citySearchTimer;
var citySearchRequestId = 0;

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
    label.textContent = city.country ? city.name + ', ' + city.country : city.name;
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

async function renderCityResults(query) {
    var requestId = ++citySearchRequestId;
    homeCityResults.innerHTML = '';

    if (!query) return;

    var cities = await fetchCitySuggestions(query);
    if (requestId !== citySearchRequestId) return;

    cities.forEach(function (city) {
        homeCityResults.appendChild(buildCityItem(city));
    });
}

homeCityTrigger.addEventListener('click', function () {
    clearTimeout(citySearchTimer);
    citySearchRequestId++;
    selectedCity = null;
    homeCitySearch.value = '';
    homeCityResults.innerHTML = '';
    homeCitySaveBtn.disabled = true;

    homeCityModal.open();
});

homeCitySearch.addEventListener('input', function () {
    clearTimeout(citySearchTimer);
    citySearchTimer = setTimeout(function () {
        renderCityResults(homeCitySearch.value);
    }, 250);
});

homeCitySaveBtn.addEventListener('click', async function () {
    if (!selectedCity) return;

    homeCitySaveBtn.disabled = true;
    var resolved = await resolveCity(selectedCity.id);

    if (!resolved) {
        homeCitySaveBtn.disabled = false;
        return;
    }

    endPlacesSession();

    homeCityIdInput.value = resolved.cityId;
    homeCityDisplayInput.value = resolved.displayName;
    homeCityRowLabel.textContent = 'Где я живу: ' + resolved.displayName;

    homeCityModal.close();
});



var avatarTrigger = document.getElementById('avatarTrigger');
var avatarInputEl = document.getElementById('avatarFileInput');
var avatarUrlInput = document.getElementById('Input_AvatarUrl');
var avatarWrap = document.querySelector('.edit-profile__avatar-wrap');

avatarTrigger.addEventListener('click', function (){
    avatarInputEl.click();
});
avatarInputEl.addEventListener('change', function (){
    var newAvatar = avatarInputEl.files[0];
    if (!newAvatar) return;

    var formData = new FormData();
    formData.append('File', newAvatar);

    fetch('/users-avatar', {method: 'POST', body: formData })
        .then(function(response) {
            if (!response.ok) throw new Error('Не удалось загрузить аватар');
            return response.json();
        })
        .then(function (data) {
            var oldAvatar = avatarWrap.querySelector('.avatar');
            var img = document.createElement('img');
            img.className = 'avatar';
            img.style.width = oldAvatar.style.width;
            img.style.height = oldAvatar.style.height;
            img.src = data.avatarUrl;

            oldAvatar.replaceWith(img);
            avatarUrlInput.value = data.avatarUrl;
        })
        .catch(function (error) {
            console.error(error);
        });

    avatarInputEl.value = '';
});
