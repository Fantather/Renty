import { createModal } from './popover.js';

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

// Рендерит отзывы и настраивает модалку "Показать ещё"
export function initReviews(loadMoreReviews) {
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

    var reviewsModal = createModal('allReviewsModal', 'closeReviewsModalBtn');
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
}
