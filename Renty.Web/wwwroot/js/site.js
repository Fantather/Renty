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
