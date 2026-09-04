using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetReviews
{
    /// <summary>
    /// Модель автора отзыва
    /// </summary>
    public class AuthorDto
    
    {
        // Полное имя
        public string FullName { get; set; } = null!;

        // Аватарка пользователя
        public string? AvatarUrl { get; set; }
    }
}
