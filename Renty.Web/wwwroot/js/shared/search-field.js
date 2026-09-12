import { createPopover } from './popover.js';

export function createSearchField({ inputId, popoverId, searchUrl, onSelect, iconFor }) {
    var input = document.getElementById(inputId);
    var dismissible = createPopover(inputId, popoverId, function () {
        renderResults(input.value);
    }, 'focus');
    var resultsPanel = dismissible.root;

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
