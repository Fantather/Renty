import { createDismissible } from './popover.js';


// inputId - Id строки ввода, по значению которой ищутся результаты
// popoverId - Id popover в который будут выведены результаты поиска
// fetchItems - callback для получения данных, должен вернуть массив
// onSelect - callback, вызывается с выбранным элементом при клике на результат. popover после него закрывается
// renderResults - необязательный callback, полностью отрисовывает содержимое popover. Есть отрисовка по умолчанию
export function createSearchField({ inputId, popoverId, fetchItems, onSelect, renderResults = null }) {
    var input = document.getElementById(inputId);
    var dismissible = createDismissible(popoverId);
    var resultsPanel = dismissible.root;
    var render = renderResults || defaultRenderResults;
    var iconTemplate = resultsPanel.querySelector('template');
    var icons = iconTemplate ? Array.from(iconTemplate.content.querySelectorAll('svg')) : [];

    document.addEventListener('click', function (e) {
        if (!resultsPanel.contains(e.target) && e.target !== input) dismissible.close();
    }, true);

    input.addEventListener('focus', function () {
        updateResults(input.value);
    });

    var debounceTimer;
    input.addEventListener('input', function () {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(function () {
            updateResults(input.value);
        }, 250);
    });

    async function search(query) {
        if (!query) return [];
        return await fetchItems(query);
    }

    async function updateResults(query) {
        var results = await search(query);

        render(resultsPanel, results, icons);

        if (results.length > 0) {
            dismissible.open();
        } else {
            dismissible.close();
        }
    }

    function defaultRenderResults(panel, results, icons) {
        panel.innerHTML = '';
        results.forEach(function (item) {
            panel.appendChild(buildItem(item, icons));
        });
    }

    function buildItem(item, icons) {
        var button = document.createElement('button');
        button.type = 'button';
        button.className = 'search-field__item';

        if (icons.length != 0) {
            var icon = document.createElement('span');
            icon.className = 'search-field__item-icon';
            icon.appendChild(icons[0].cloneNode(true));
            button.appendChild(icon);
        }

        var title = document.createElement('span');
        title.className = 'search-field__item-title';
        title.textContent = item.title;
        button.appendChild(title);

        button.addEventListener('click', function () {
            onSelect(item);
            dismissible.close();
        });

        return button;
    }

    return dismissible;
}
