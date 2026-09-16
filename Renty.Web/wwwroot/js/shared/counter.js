export function createCounter(options) {
    var rows = options.rows;
    var initialCounts = options.initialCounts || {};
    var onChange = options.onChange;

    var counts = {};

    rows.forEach(function (row) {
        var key = row.dataset.counter;
        var min = parseInt(row.dataset.counterMin, 10) || 0;
        var countEl = row.querySelector('.counter-row__count');
        var minusBtn = row.querySelector('.counter-row__btn--minus');
        var plusBtn = row.querySelector('.counter-row__btn--plus');

        counts[key] = initialCounts[key] || min;
        renderCounter(key, countEl, minusBtn, min);

        minusBtn.addEventListener('click', function () {
            counts[key] = Math.max(min, counts[key] - 1);
            renderCounter(key, countEl, minusBtn, min);
            if (onChange) onChange(counts);
        });

        plusBtn.addEventListener('click', function () {
            counts[key]++;
            renderCounter(key, countEl, minusBtn, min);
            if (onChange) onChange(counts);
        });
    });

    function renderCounter(key, countEl, minusBtn, min) {
        countEl.textContent = counts[key];
        minusBtn.disabled = counts[key] <= min;
    }

    return { counts: counts };
}
