import { createDismissible } from './popover.js';

export function createSearchField({ inputId, popoverId, searchUrl, onSelect, iconFor }) {
    var input = document.getElementById(inputId);
    var dismissible = createDismissible(popoverId);
    var resultsPanel = dismissible.root;

    document.addEventListener('click', function (e) {
        if (!resultsPanel.contains(e.target) && e.target !== input) dismissible.close();
    }, true);

    input.addEventListener('focus', function () {
        renderResults(input.value);
    });

    var debounceTimer;
    input.addEventListener('input', function () {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(function () {
            renderResults(input.value);
        }, 250);
    });

    async function search(query) {
        if (!query) return [];

        var res = await fetch(searchUrl + '?searchTerm=' + encodeURIComponent(query));
        if (!res.ok) return [];

        return await res.json();
    }

    async function renderResults(query) {
        var results = await search(query);
        resultsPanel.innerHTML = '';
        results.forEach(function (item) {
            resultsPanel.appendChild(buildItem(item));
        });

        if (results.length > 0) {
            dismissible.open();
        } else {
            dismissible.close();
        }
    }

    function buildItem(item) {
        var button = document.createElement('button');
        button.type = 'button';
        button.className = 'search-field__item';

        if (iconFor) {
            var icon = document.createElement('span');
            icon.className = 'search-field__item-icon';
            icon.innerHTML = iconFor(item);
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
