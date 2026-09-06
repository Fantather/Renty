import { createDismissible } from '../shared/popover.js';

var fullGalleryOverlay = document.getElementById('fullGalleryOverlay');
var closeFullGalleryBtn = document.getElementById('closeFullGalleryBtn');

if (fullGalleryOverlay && closeFullGalleryBtn) {
    var galleryTriggers = document.querySelectorAll('.property-gallery__main, .property-gallery__thumb, .room-card__image');
    galleryTriggers.forEach(function (img) {
        img.addEventListener('click', function () {
            fullGalleryOverlay.hidden = false;
        });
    });

    closeFullGalleryBtn.addEventListener('click', function () {
        fullGalleryOverlay.hidden = true;
    });
}

var lightboxImage = document.getElementById('lightboxImage');
var lightboxCounter = document.getElementById('lightboxCounter');
var lightboxPrevBtn = document.getElementById('lightboxPrevBtn');
var lightboxNextBtn = document.getElementById('lightboxNextBtn');
var lightboxOverlay = document.getElementById('lightboxOverlay');

if (lightboxOverlay && lightboxImage) {
    var lightboxImages = Array.from(document.querySelectorAll('.full-gallery__grid img'));
    var lightbox = createDismissible('lightboxOverlay', null);
    var currentLightboxIndex = 0;

    function showLightboxImage(index) {
        currentLightboxIndex = index;
        lightboxImage.src = lightboxImages[index].src;
        lightboxCounter.textContent = (index + 1) + ' з ' + lightboxImages.length;
    }

    lightboxImages.forEach(function (img, index) {
        img.addEventListener('click', function () {
            showLightboxImage(index);
            lightbox.open();
        });
    });

    document.getElementById('closeLightboxBtn').addEventListener('click', lightbox.close);

    lightboxPrevBtn.addEventListener('click', function () {
        showLightboxImage((currentLightboxIndex - 1 + lightboxImages.length) % lightboxImages.length);
    });
    lightboxNextBtn.addEventListener('click', function () {
        showLightboxImage((currentLightboxIndex + 1) % lightboxImages.length);
    });

    document.addEventListener('keydown', function (e) {
        if (lightboxOverlay.hidden) return;
        if (e.key === 'ArrowLeft') lightboxPrevBtn.click();
        if (e.key === 'ArrowRight') lightboxNextBtn.click();
    });
}
