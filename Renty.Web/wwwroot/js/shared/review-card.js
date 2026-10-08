var STAR_SVG = '<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M12 2L15.5188 7.66667L22 9.25L17.6885 14.3472L18.1752 21L12 18.4722L5.82476 21L6.32545 14.3472L2 9.25L8.48122 7.66667L12 2Z" fill="currentColor"/></svg>';
var FIVE_STARS = STAR_SVG.repeat(5);

function buildStars(rating) {
    var stars = document.createElement('span');
    stars.className = 'review-card__stars';
    stars.innerHTML = FIVE_STARS;

    var fill = document.createElement('span');
    fill.className = 'review-card__stars-fill';
    fill.innerHTML = FIVE_STARS;
    fill.style.width = Math.min(Math.max(rating, 0), 5) / 5 * 100 + '%';

    stars.appendChild(fill);
    return stars;
}

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

    authorInfo.appendChild(author);

    header.appendChild(avatar);
    header.appendChild(authorInfo);

    var meta = document.createElement('div');
    meta.className = 'review-card__meta';

    var separator = document.createElement('span');
    separator.className = 'review-card__date';
    separator.textContent = '·';

    var date = document.createElement('span');
    date.className = 'review-card__date';
    var createdAt = new Date(review.createdAt);
    date.textContent = createdAt.toLocaleDateString('ru-RU', { day: 'numeric', month: 'long' }) + ' ' + createdAt.getFullYear();

    meta.appendChild(buildStars(review.rating));
    meta.appendChild(separator);
    meta.appendChild(date);

    var text = document.createElement('p');
    text.className = 'review-card__text';
    text.textContent = review.text;

    card.appendChild(header);
    card.appendChild(meta);
    card.appendChild(text);

    if (review.propertySlug && review.propertyName) {
        var propertyLink = document.createElement('a');
        propertyLink.className = 'review-card__property';
        propertyLink.href = '/properties/' + encodeURIComponent(review.propertySlug);
        propertyLink.textContent = review.propertyName;
        card.appendChild(propertyLink);
    }

    return card;
}
