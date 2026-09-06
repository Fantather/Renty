var propertyNav = document.querySelector('.property-nav');
var navTrigger = document.getElementById('navTrigger');

if (propertyNav && navTrigger) {
    var navObserver = new IntersectionObserver(function (entries) {
        var entry = entries[0];
        if (entry.isIntersecting) {
            propertyNav.classList.remove('is-visible');
        } else if (entry.boundingClientRect.top < 0) {
            propertyNav.classList.add('is-visible');
        }
    });
    navObserver.observe(navTrigger);
}
