using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Характеристики недвижимости с поддержкой истории изменений
    /// Используется паттерн Effective Dating для отслеживания изменений во времени
    /// </summary>
    public class PropertyDetails
    {
        public Guid Id { get; set; }

        // Связь с недвижимостью
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        // Характеристики
        public int MaxGuests { get; set; } = 1;

        public int BedsCount { get; set; } = 0;

        public int BedroomsCount { get; set; } = 0;

        public int BathroomsCount { get; set; } = 0;

        // период действия этих характеристик
        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;

        // null = текущая версия
        public DateTime? ValidTo { get; set; }

        // Кто изменил
        public Guid? ModifiedByUserId { get; set; }

        // Когда создана запись
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
