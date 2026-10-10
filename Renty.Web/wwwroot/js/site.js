// Не перепроверять поле на каждый символ при повторном вводе
$.validator.setDefaults({
    onkeyup: false
});

// Счётчик символов для .input-field с заданным maxlength
document.querySelectorAll('.input-field__input[maxlength]').forEach(function (input) {
    var counter = input.closest('.input-field').querySelector('.input-field__counter');
    if (!counter) return;

    function updateCounter() {
        counter.textContent = input.value.length + '/' + input.getAttribute('maxlength');
    }

    input.addEventListener('input', updateCounter);
    updateCounter();
});

document.addEventListener('click', function (e) {
    var button = e.target.closest('button[formnovalidate][form]');
    if (!button) return;

    var form = document.getElementById(button.getAttribute('form'));
    var validator = form && $(form).data('validator');
    if (validator) validator.cancelSubmit = true;
});
