using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetReviews
{
    public class ReviewDto
    // Модель отзыва
    {
        public Guid Id { get; set; }

        public AuthorDto Author { get; set; } = null!;

        public string Content { get; set; } = null!;

        // бщая оценка (1-5)
        public decimal Rating { get; set; }

        // Детальные оценки (1-5 каждая)
        public decimal? CleanlinessRating { get; set; }
        public decimal? CommunicationRating { get; set; }
        public decimal? AccuracyRating { get; set; }
        public decimal? LocationRating { get; set; }

        // Ответ хоста на отзыв
        public HostResponseDto? HostResponse { get; set; }

        // Даты
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
