var STORAGE_KEY = 'editorSidebarScroll';
var sidebar = document.querySelector('.editor__sidebar');

if (sidebar) {
    window.addEventListener('pagehide', function () {
        try {
            sessionStorage.setItem(STORAGE_KEY, sidebar.scrollTop);
        } catch (e) { }
    });
}
