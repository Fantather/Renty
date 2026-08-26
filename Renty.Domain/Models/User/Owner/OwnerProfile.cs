using System;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.User;

namespace Renty.Domain.Models.User.Host
{
    /// <summary>
    /// Профиль хоста с базовой информацией 
    /// </summary>
    public class OwnerProfile
    {
        public int Id { get; set; }

        // Связь с пользователем(личный аккаут владельца)
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        // Информация о владельце
        public string? Bio { get; set; }

        public string? Languages { get; set; } // Например: "English, Spanish, French"


        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }


        // Коллекция объектов недвижимости, которыми владеет челиксон
        public ICollection<Properties.Property> Properties { get; set; } = new List<Properties.Property>();
    }
}
