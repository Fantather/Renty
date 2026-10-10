export function createCounter(options) {
    var rows = options.rows;
    var initialCounts = options.initialCounts || {};
    var onChange = options.onChange;

    var counts = {};

    rows.forEach(function (row) {
        var key = row.dataset.counter;
        var min = parseInt(row.dataset.counterMin, 10) || 0;
        var max = parseInt(row.dataset.counterMax, 10) || Infinity;
        var countEl = row.querySelector('.popover-row__count');
        var minusBtn = row.querySelector('.popover-row__btn--minus');
        var plusBtn = row.querySelector('.popover-row__btn--plus');

        counts[key] = Math.min(max, initialCounts[key] || min);
        renderCounter(key, countEl, minusBtn, plusBtn, min, max);

        minusBtn.addEventListener('click', function () {
            counts[key] = Math.max(min, counts[key] - 1);
            renderCounter(key, countEl, minusBtn, plusBtn, min, max);
            if (onChange) onChange(counts);
        });

        plusBtn.addEventListener('click', function () {
            counts[key] = Math.min(max, counts[key] + 1);
            renderCounter(key, countEl, minusBtn, plusBtn, min, max);
            if (onChange) onChange(counts);
        });
    });

    function renderCounter(key, countEl, minusBtn, plusBtn, min, max) {
        countEl.textContent = counts[key];
        minusBtn.disabled = counts[key] <= min;
        plusBtn.disabled = counts[key] >= max;
    }

    return { counts: counts };
}
