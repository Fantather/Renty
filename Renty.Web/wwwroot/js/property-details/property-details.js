import { createModal } from '../shared/popover.js';

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

var amenitiesModal = createModal('allAmenitiesModal', 'closeAmenitiesModalBtn');

var showAllAmenitiesBtn = document.getElementById('showAllAmenitiesBtn');
if (showAllAmenitiesBtn) {
    showAllAmenitiesBtn.addEventListener('click', amenitiesModal.open);
}

// TODO: заменить на реальный запрос, когда на бэке появится контроллер поверх GetPropertyReviewsQuery
// async function loadMoreReviews(propertyId) {
//     var res = await fetch('/api/properties/' + propertyId + '/reviews?skip=4');
//     return await res.json();
// }
async function loadMoreReviews() {
    return [
        { authorName: 'Оксана', authorAvatarUrl: 'https://placehold.co/60x60', text: 'Все сподобалось, обов’язково повернемось.' },
        { authorName: 'Дмитро', authorAvatarUrl: 'https://placehold.co/60x60', text: 'Чисто, тихо, поруч з морем.' },
    ];
}

function buildReviewCard(review) {
    var card = document.createElement('div');
    card.className = 'review-card';

    var header = document.createElement('div');
    header.className = 'review-card__header';

    var avatar = document.createElement('img');
    avatar.className = 'review-card__avatar';
    avatar.src = review.authorAvatarUrl;
    avatar.alt = review.authorName;

    var author = document.createElement('p');
    author.className = 'review-card__author';
    author.textContent = review.authorName;

    header.appendChild(avatar);
    header.appendChild(author);

    var text = document.createElement('p');
    text.className = 'review-card__text';
    text.textContent = review.text;

    card.appendChild(header);
    card.appendChild(text);
    return card;
}

var loadMoreBtn = document.getElementById('loadMoreReviewsBtn');
var reviewsList = document.getElementById('reviewsList');

if (loadMoreBtn) {
    loadMoreBtn.addEventListener('click', async function () {
        var extraReviews = await loadMoreReviews();
        extraReviews.forEach(function (review) {
            reviewsList.appendChild(buildReviewCard(review));
        });
        loadMoreBtn.hidden = true;
    });
}
