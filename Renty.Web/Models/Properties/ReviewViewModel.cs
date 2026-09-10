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
    }
}
