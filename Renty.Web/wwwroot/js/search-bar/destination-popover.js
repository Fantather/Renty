import { createPopover } from '../shared/popover.js';

// Popover
var destinationInput = document.getElementById('destinationInput');
var destinationDismissible = createPopover('destinationInput', 'destinationPopover', function () {
    renderResults(destinationInput.value);
}, 'focus');
var destinationPopover = destinationDismissible.root;

destinationInput.addEventListener('input', function () {
    renderResults(destinationInput.value);
});

// Mock data
// TODO: когда бэкенд будет готов, searchDestinations должна дёргать реальный эндпоинт вместо фильтрации мока.
var mockDestinations = [
    { id: 'rec-1', title: 'Поблизости', subtitle: 'Узнать, что есть поблизости' },
    { id: 'rec-2', title: 'Париж', subtitle: 'Гости, которых интересовал г. Барселона, также заглянули сюда' },
    { id: 'rec-3', title: 'Одесса', subtitle: 'Вы часто приезжаете сюда' },
    { id: 'rec-4', title: 'Валенсия', subtitle: 'Для всей семьи' },
    { id: 'rec-5', title: 'Киев', subtitle: 'Популярное направление' }
];

async function searchDestinations(query) {
    // TODO: заменить на реальный вызов, когда бэкенд будет готов:
    // const res = await fetch('/api/cities?search=' + encodeURIComponent(query));
    // const data = await res.json();
    // return data.cities.map(c => ({ id: c.cityId, title: c.cityName, subtitle: c.countryName }));

    if (!query) return mockDestinations;

    var q = query.toLowerCase();
    return mockDestinations.filter(function (d) {
        return d.title.toLowerCase().includes(q);
    });
}

// Rendering
async function renderResults(query) {
    var results = await searchDestinations(query);
    destinationPopover.innerHTML = '';

    if (!query) {
        appendGroup('Рекомендуемые направления', results);
    } else {
        results.forEach(function (d) {
            destinationPopover.appendChild(buildItem(d));
        });
    }
}

function appendGroup(heading, items) {
    if (!items.length) return;

    var headingEl = document.createElement('p');
    headingEl.className = 'destination-popover__heading';
    headingEl.textContent = heading;
    destinationPopover.appendChild(headingEl);

    items.forEach(function (d) {
        destinationPopover.appendChild(buildItem(d));
    });
}

function buildItem(destination) {
    var item = document.createElement('button');
    item.type = 'button';
    item.className = 'destination-popover__item';

    var icon = document.createElement('span');
    icon.className = 'destination-popover__icon';
    item.appendChild(icon);

    var info = document.createElement('span');
    info.className = 'destination-popover__info';

    var title = document.createElement('span');
    title.className = 'destination-popover__title';
    title.textContent = destination.title;
    info.appendChild(title);

    var subtitle = document.createElement('span');
    subtitle.className = 'destination-popover__subtitle';
    subtitle.textContent = destination.subtitle;
    info.appendChild(subtitle);

    item.appendChild(info);

    item.addEventListener('click', function () {
        destinationInput.value = destination.title;
        destinationDismissible.close();
    });

    return item;
}
