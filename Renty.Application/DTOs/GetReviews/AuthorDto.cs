using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetReviews
{
    public class AuthorDto
    // Модель автора отзыва
    {
        // Полное имя
        public string FullName { get; set; } = null!;

        // Аватарка пользователя
        public string? AvatarUrl { get; set; }
    }
}
