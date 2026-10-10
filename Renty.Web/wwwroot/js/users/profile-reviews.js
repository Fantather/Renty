import { createModal } from '../shared/popover.js';
import { buildReviewCard } from '../shared/review-card.js';

var VISIBLE_REVIEWS_COUNT = 4;

async function loadReviewsPage(page, userId) {
    var response = await fetch('/users/' + userId + '/reviews?page=' + page);
    if (!response.ok) return { reviews: [], currentPage: page, totalPages: 0 };
    return await response.json();
}

var reviewsList = document.getElementById('profileReviewsList');
var allReviewsList = document.getElementById('profileAllReviewsList');
var reviewsDataEl = document.getElementById('profileReviewsData');

var reviewsPage = JSON.parse(reviewsDataEl.textContent);

if (reviewsList && allReviewsList) {
    reviewsPage.reviews.forEach(function (review, index) {
        if (index < VISIBLE_REVIEWS_COUNT) {
            reviewsList.appendChild(buildReviewCard(review));
        }
        allReviewsList.appendChild(buildReviewCard(review));
    });
}

var loadMoreBtn = document.getElementById('profileLoadMoreReviewsBtn');

if (loadMoreBtn) {
    var reviewsModal = createModal('profileReviewsModal', 'closeProfileReviewsModalBtn');
    loadMoreBtn.addEventListener('click', reviewsModal.open);

    var sentinel = document.getElementById('profileReviewsSentinel');
    var currentPage = reviewsPage.currentPage;
    var totalPages = reviewsPage.totalPages;
    var userId = document.getElementById('profileReviewsModal').dataset.userId;
    var isLoading = false;

    var observer = new IntersectionObserver(async function (entries) {
        if (!entries[0].isIntersecting || isLoading) return;

        if (currentPage >= totalPages) {
            observer.disconnect();
            return;
        }

        isLoading = true;
        var data = await loadReviewsPage(currentPage + 1, userId);
        data.reviews.forEach(function (review) {
            allReviewsList.appendChild(buildReviewCard(review));
        });
        currentPage = data.currentPage;
        totalPages = data.totalPages;
        isLoading = false;

        if (data.reviews.length === 0 || currentPage >= totalPages) {
            observer.disconnect();
            return;
        }

        observer.unobserve(sentinel);
        observer.observe(sentinel);
    });

    observer.observe(sentinel);
}
