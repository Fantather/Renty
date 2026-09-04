using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    /// <summary>
    /// Автор жилья для аренды
    /// </summary>
    public class HostDto

    {
        public string? AvatarUrl { get; set; }

        // Полное имя
        public string FullName { get; set; } = null!;
        
        // Дата создания аккаунта
        public DateTime CreatedAt { get; set; }

        // Языки которые знает пользователь
        public string Languages { get; set; } = null!;

        // Скорость ответа
        public string ResponseSpeed { get; set; } = null!;

        // Верифицирован ли пользователь
        public bool IsVerified { get; set; }

        // Информация о пользователе
        public string? Info { get; set; }
    }
}
