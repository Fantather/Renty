import { createPopover } from '../shared/popover.js';
import { loginModal } from '../login-modal/login-modal.js';

var mainMenu = createPopover('mainMenuTrigger', 'mainMenuPopover', null);

var loginTrigger = document.getElementById('loginTrigger');
if (loginTrigger) {
    loginTrigger.addEventListener('click', function () {
        mainMenu.close();
        loginModal.open();
    });
}

var logoutButton = document.getElementById('logoutButton');
if (logoutButton) {
    logoutButton.addEventListener('click', function () {
        // TODO: реальный выход, когда подключим Identity
        mainMenu.close();
    });
}
