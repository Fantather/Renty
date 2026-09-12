export function limitCheckboxSelection(checkboxes, max) {
    var selected = [];

    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            if (checkbox.checked) {
                selected.push(checkbox);
                if (selected.length > max) {
                    var oldest = selected.shift();
                    oldest.checked = false;
                }
            } else {
                selected = selected.filter(function (c) { return c !== checkbox; });
            }
        });
    });
}
