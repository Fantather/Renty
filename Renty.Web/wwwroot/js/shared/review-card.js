export function buildReviewCard(review) {
    var card = document.createElement('div');
    card.className = 'review-card';

    var header = document.createElement('div');
    header.className = 'review-card__header';

    var avatar;
    if (review.authorAvatarUrl) {
        avatar = document.createElement('img');
        avatar.className = 'review-card__avatar';
        avatar.src = review.authorAvatarUrl;
        avatar.alt = review.authorName;
    } else {
        avatar = document.createElement('div');
        avatar.className = 'review-card__avatar avatar--placeholder';
        var initial = document.createElement('span');
        initial.textContent = review.authorName ? review.authorName[0].toUpperCase() : '?';
        avatar.appendChild(initial);
    }

    var authorInfo = document.createElement('div');

    var author = document.createElement('p');
    author.className = 'review-card__author';
    author.textContent = review.authorName;

    var date = document.createElement('p');
    date.className = 'review-card__date';
    var createdAt = new Date(review.createdAt);
    date.textContent = createdAt.toLocaleDateString('ru-RU', { day: 'numeric', month: 'long' }) + ' ' + createdAt.getFullYear();

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
