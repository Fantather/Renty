import { createPopover } from './popover.js';

export function createCustomSelect(select) {
    var wrapper = document.createElement('div');
    wrapper.className = 'custom-select';
    select.parentNode.insertBefore(wrapper, select);
    wrapper.appendChild(select);

    var panel = document.createElement('div');
    panel.id = select.id + 'Popover';
    panel.className = 'popover-panel custom-select__panel';
    wrapper.appendChild(panel);

    Array.from(select.options).forEach(function (option) {
        if (!option.value) return;
        var item = document.createElement('button');
        item.type = 'button';
        item.className = 'custom-select__option';
        item.textContent = option.text;
        item.dataset.value = option.value;
        panel.appendChild(item);
    });

    var popover = createPopover(select.id, panel.id, onOpen);

    select.addEventListener('mousedown', function (e) {
        e.preventDefault();
        select.focus();
    });

    select.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' || e.key === ' ' || (e.altKey && e.key === 'ArrowDown')) {
            e.preventDefault();
            popover.open();
        }
    });

    panel.addEventListener('click', function (e) {
        var item = e.target.closest('.custom-select__option');
        if (!item) return;

        select.value = item.dataset.value;
        select.dispatchEvent(new Event('change', { bubbles: true }));
        if (window.jQuery && $(select.form).data('validator')) $(select).valid();

        popover.close();
        select.focus();
    });

    function onOpen() {
        panel.querySelectorAll('.custom-select__option').forEach(function (item) {
            item.classList.toggle('is-selected', item.dataset.value === select.value);
        });

        var selected = panel.querySelector('.is-selected');
        panel.scrollTop = selected
            ? selected.offsetTop - (panel.clientHeight - selected.offsetHeight) / 2
            : 0;
    }

    return popover;
}
