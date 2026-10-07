export function submitFormAsync(form, url, errorsList) {
    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        var res = await fetch(url, { method: 'POST', body: new FormData(form) });

        if (res.ok) {
            location.reload();
            return;
        }

        var errors = ['Не удалось сохранить'];
        if (res.status === 400) {
            var body = await res.json();
            errors = body.errors;
        }

        errorsList.innerHTML = '';
        errors.forEach(function (message) {
            var item = document.createElement('li');
            item.textContent = message;
            errorsList.appendChild(item);
        });
    });
}
