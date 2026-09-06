import { createModal } from '../shared/popover.js';

// TODO: заменить на реальный запрос, когда на бэке появится контроллер поверх GetPropertyReviewsQuery
// async function loadMoreReviews(propertyId) {
//     var res = await fetch('/api/properties/' + propertyId + '/reviews?skip=4');
//     return await res.json();
// }
async function loadMoreReviews() {
    return [
        { authorName: 'Оксана', authorAvatarUrl: 'https://placehold.co/60x60', text: 'Все понравилось, обязательно вернёмся.', createdAt: '2026-08-10' },
        { authorName: 'Дмитрий', authorAvatarUrl: 'https://placehold.co/60x60', text: 'Чисто, тихо, рядом с морем.', createdAt: '2026-08-02' },
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

    var authorInfo = document.createElement('div');

    var author = document.createElement('p');
    author.className = 'review-card__author';
    author.textContent = review.authorName;

    var date = document.createElement('p');
    date.className = 'review-card__date';
    date.textContent = new Date(review.createdAt).toLocaleDateString('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' });

    authorInfo.appendChild(author);
    authorInfo.appendChild(date);

    header.appendChild(avatar);
    header.appendChild(authorInfo);

    var text = document.createElement('p');
    text.className = 'review-card__text';
    text.textContent = review.text;

    card.appendChild(header);
    card.appendChild(text);
    return card;
}

var reviewsModal = createModal('allReviewsModal', 'closeReviewsModalBtn');

var reviewsList = document.getElementById('reviewsList');
var allReviewsList = document.getElementById('allReviewsList');
var initialReviewsDataEl = document.getElementById('initialReviewsData');

if (reviewsList && allReviewsList && initialReviewsDataEl) {
    var initialReviews = JSON.parse(initialReviewsDataEl.textContent);
    initialReviews.forEach(function (review) {
        reviewsList.appendChild(buildReviewCard(review));
        allReviewsList.appendChild(buildReviewCard(review));
    });
}

var loadMoreBtn = document.getElementById('loadMoreReviewsBtn');
var extraReviewsLoaded = false;

if (loadMoreBtn) {
    loadMoreBtn.addEventListener('click', async function () {
        reviewsModal.open();

        if (!extraReviewsLoaded) {
            extraReviewsLoaded = true;
            var extraReviews = await loadMoreReviews();
            extraReviews.forEach(function (review) {
                allReviewsList.appendChild(buildReviewCard(review));
            });
        }
    });
}
