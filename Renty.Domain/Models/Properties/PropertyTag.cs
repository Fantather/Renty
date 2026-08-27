using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Связь многие-ко-многим между недвижимостью и тегами
    /// </summary>
    public class PropertyTag
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        public int TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; }

        // Дата добавления тега к недвижимости
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
