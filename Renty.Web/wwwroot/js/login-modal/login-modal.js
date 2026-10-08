import { createModal } from '../shared/popover.js';

export var loginModal = createModal('loginModal', 'loginModalClose');

var url = new URL(window.location.href);
if (url.searchParams.has('login')) {
    url.searchParams.delete('login');
    history.replaceState(null, '', url);
    loginModal.open();
}
var form = document.getElementById('loginForm');
form.addEventListener('submit', function(event) {
    event.preventDefault();

    var result = $(form).valid();
    if (result) {
        var formData = new FormData(form);
        fetch(form.action, { method: 'POST', body: formData })
            .then(function(response) { return response.json(); })
            .then(function(data) {
                if (data.success === true) {
                    window.location.href = data.returnUrl;
                } else {
                    var errorBox = document.getElementById('loginServerError');
                    errorBox.textContent = data.error;
                    errorBox.hidden = false;
                }
            });
    }
});