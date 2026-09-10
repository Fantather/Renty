export function createGuestCounter(options) {
    var rows = options.rows;
    var initialCounts = options.initialCounts || {};
    var onChange = options.onChange;

    var counts = {};

    rows.forEach(function (row) {
        var key = row.dataset.counter;
        var countEl = row.querySelector('.guests-popover__count');
        var minusBtn = row.querySelector('.guests-popover__btn--minus');
        var plusBtn = row.querySelector('.guests-popover__btn--plus');

        counts[key] = initialCounts[key] || 0;
        renderCounter(key, countEl, minusBtn);

        minusBtn.addEventListener('click', function () {
            counts[key] = Math.max(0, counts[key] - 1);
            renderCounter(key, countEl, minusBtn);
            if (onChange) onChange(counts);
        });

        plusBtn.addEventListener('click', function () {
            counts[key]++;
            renderCounter(key, countEl, minusBtn);
            if (onChange) onChange(counts);
        });
    });

    function renderCounter(key, countEl, minusBtn) {
        countEl.textContent = counts[key];
        minusBtn.disabled = counts[key] === 0;
    }

    return { counts: counts };
}
