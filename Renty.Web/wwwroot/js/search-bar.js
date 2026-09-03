(
    function() {
        var dateSegment = document.getElementById('dateSegment');
        var datePopover = document.getElementById('datePopover');

        dateSegment.addEventListener('click', function() {
            datePopover.hidden ? open() : close();
        })


        function open() {
            datePopover.hidden = false;
            document.addEventListener('click', onOutsideClick, true);
            document.addEventListener('keydown', onEscape);
        }
        function close() {
            datePopover.hidden = true;
            document.removeEventListener('click', onOutsideClick, true);
            document.removeEventListener('keydown', onEscape);
        }

        function onOutsideClick(e) {
            if (!datePopover.contains(e.target) && !dateSegment.contains(e.target)) close();
        }
        function onEscape(e) {
            if (e.key == 'Escape') close();
        }
    }
)();