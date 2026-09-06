namespace Renty.Web.Models.Properties
{
    // Один отзыв гостя
    public class ReviewViewModel
    {
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorAvatarUrl { get; set; }
        public decimal Rating { get; set; } // оценка этого отзыва (1-5)
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // В БД (Review) сейчас нет текстовых полей под комментарии к отдельным категориям —
        // только числовые оценки (CleanlinessRating/CommunicationRating/AccuracyRating/LocationRating)
        public string? CleanlinessComment { get; set; } // комментарий к чистоте
        public string? AccuracyComment { get; set; } // комментарий к соответствию описанию/фото
        public string? CheckInComment { get; set; } // комментарий к заезду
        public string? CommunicationComment { get; set; } // комментарий к общению с хозяином
        public string? LocationComment { get; set; } // комментарий к расположению
        public string? ValueComment { get; set; } // комментарий к цене/качеству
    }
}
