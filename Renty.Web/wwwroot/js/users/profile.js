import { initReviews } from '../shared/reviews.js';

// TODO: заменить на реальный запрос, когда на бэке появится контроллер поверх GetUserReviewsQuery
async function loadMoreReviews() {
    return [
        { authorName: 'Марина', authorAvatarUrl: 'https://placehold.co/60x60', text: 'Отличный хозяин, всё было готово к заезду.', createdAt: '2026-07-28' },
        { authorName: 'Артём', authorAvatarUrl: 'https://placehold.co/60x60', text: 'Быстро отвечает, помог с заселением поздно вечером.', createdAt: '2026-07-15' },
    ];
}

initReviews(loadMoreReviews);
