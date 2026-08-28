using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties;

namespace Renty.Domain.Models.User
{
    /// <summary>
    /// Избранные объекты недвижимости пользователей
    /// </summary>
    public class Favorite
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Пользователь
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        // Недвижимость
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        // Дата добавления в избранное
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
