using System;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties;

namespace Renty.Domain.Models.User
{
    /// <summary>
    /// Отзывы пользователей о недвижимости (упрощенная версия)
    /// </summary>
    public class Review
    {
        public int Id { get; set; }

        // Пользователь, который оставил отзыв
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        // Недвижимость, на которую оставлен отзыв
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        // Общая оценка (1-5) - вычисляется на основе детальных оценок или задается независимо
        public decimal Rating { get; set; }

        // Детальные оценки (1-5 каждая)
        public decimal? CleanlinessRating { get; set; }

        public decimal? CommunicationRating { get; set; }

        public decimal? AccuracyRating { get; set; }

        public decimal? LocationRating { get; set; }

        // Текст отзыва
        public string Comment { get; set; } = string.Empty;

        // Ответ хоста на отзыв (он отображается обычно вроде)
        public string? HostResponse { get; set; }

        public DateTime? HostResponseDate { get; set; }

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
