export var monthNames = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];

export function formatShortDate(d) {
    return d.getDate() + ' ' + monthNames[d.getMonth()].slice(0, 3);
}

export function formatIsoDate(d) {
    return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
}

function pad(n) {
    return n < 10 ? '0' + n : '' + n;
}
