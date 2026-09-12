// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Не перепроверять поле на каждую букву при повторном вводе — мешает исправлять.
// Проверка по уходу с поля (onfocusout) остаётся как есть.
$.validator.setDefaults({
    onkeyup: false
});
