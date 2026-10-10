import { createModal } from '../shared/popover.js';
import { buildReviewCard } from '../shared/review-card.js';

var VISIBLE_REVIEWS_COUNT = 4;

// TODO: заменить на реальный запрос, когда на бэке появится контроллер поверх GetPropertyReviewsQuery
// async function loadMoreReviews(propertyId) {
//     var res = await fetch('/api/properties/' + propertyId + '/reviews?skip=4');
//     return await res.json();
// }
async function loadMoreReviews() {
    return [];
}

var reviewsModal = createModal('allReviewsModal', 'closeReviewsModalBtn');

var reviewsList = document.getElementById('reviewsList');
var allReviewsList = document.getElementById('allReviewsList');
var initialReviewsDataEl = document.getElementById('initialReviewsData');

if (reviewsList && allReviewsList && initialReviewsDataEl) {
    var initialReviews = JSON.parse(initialReviewsDataEl.textContent).reviews;
    initialReviews.forEach(function (review, index) {
        if (index < VISIBLE_REVIEWS_COUNT) {
            reviewsList.appendChild(buildReviewCard(review));
        }
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
