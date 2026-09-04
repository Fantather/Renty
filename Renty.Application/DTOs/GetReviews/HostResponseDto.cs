using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetReviews
{
    /// <summary>
    /// Модель ответа владельца на отзыв
    /// </summary>
    public class HostResponseDto
    
    {
        public AuthorDto Host { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
