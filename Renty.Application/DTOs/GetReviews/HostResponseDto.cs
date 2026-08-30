using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetReviews
{
    public class HostResponseDto
    // Модель ответа владельца на отзыв
    {
        public AuthorDto Host { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
