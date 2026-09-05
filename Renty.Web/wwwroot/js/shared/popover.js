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

export function createPopover(triggerId, rootId, onOpen) {
    var trigger = document.getElementById(triggerId);
    var dismissible = createDismissible(rootId, onOpen);

    trigger.addEventListener('click', function () {
        dismissible.root.hidden ? open() : close();
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
