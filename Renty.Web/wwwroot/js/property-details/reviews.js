import { initReviews } from '../shared/reviews.js';

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

initReviews(loadMoreReviews);
