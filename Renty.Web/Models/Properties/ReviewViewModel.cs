namespace Renty.Web.Models.Properties
{
    public class ReviewViewModel
    {
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorAvatarUrl { get; set; }
        public decimal Rating { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // В БД (Review) сейчас нет текстовых полей под комментарии к отдельным категориям —
        // только числовые оценки (CleanlinessRating/CommunicationRating/AccuracyRating/LocationRating).
        // Нужно попросить Ольгу добавить текстовые поля под каждую категорию (см. ToDo.txt на рабочем столе).
        public List<ReviewCategoryCommentViewModel> CategoryComments { get; set; } = new();
    }
}
