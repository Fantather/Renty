export function createDismissible(rootId, onOpen) {
    var root = document.getElementById(rootId);
    root.hidden = true;

    function open() {
        root.hidden = false;
        if (onOpen) onOpen();

        document.addEventListener('keydown', onEscape);
    }

    function close() {
        root.hidden = true;
        document.removeEventListener('keydown', onEscape);
    }
    function onEscape(e) { if (e.key == 'Escape') close(); }

    return { open: open, close: close, root: root };
}

export function createPopover(triggerId, rootId, onOpen, triggerEvent) {
    triggerEvent = triggerEvent || 'click';
    var trigger = document.getElementById(triggerId);
    var dismissible = createDismissible(rootId, onOpen);

    trigger.addEventListener(triggerEvent, function () {
        if (triggerEvent === 'focus') {
            if (dismissible.root.hidden) open();
        } else {
            dismissible.root.hidden ? open() : close();
        }
    });

    function open() {
        dismissible.open();
        document.addEventListener('click', onOutsideClick, true);
    }
    function close() {
        dismissible.close();
        document.removeEventListener('click', onOutsideClick, true);
    }
    function onOutsideClick(e) {
        if (!dismissible.root.contains(e.target) && !trigger.contains(e.target)) close();
    }

    return { open: open, close: close, root: dismissible.root, trigger: trigger };
}

export function createModal(rootId, closeButtonId) {
    var dismissible = createDismissible(rootId, null);
    var closeBtn = document.getElementById(closeButtonId);

    closeBtn.addEventListener('click', dismissible.close);

    dismissible.root.addEventListener('click', function (e) {
        if (e.target === dismissible.root) dismissible.close();
    });

    return dismissible;
}
