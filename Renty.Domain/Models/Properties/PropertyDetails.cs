using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Характеристики недвижимости с поддержкой истории изменений
    /// </summary>
    public class PropertyDetails
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Связь с недвижимостью
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        // Характеристики
        public int MaxGuests { get; set; } = 1;

        public int BedsCount { get; set; } = 0;

        public int BedroomsCount { get; set; } = 0;

        public int BathroomsCount { get; set; } = 0;

        // Количество этажей в здании
        // (Для квартиры - этажность всего дома, для частного дома - количество этажей в доме)
        public int FloorsCount { get; set; } = 1;

        // Этаж, на котором находится объект
        // (Может быть null, если это частный дом, вилла или отель целиком)
        public int? Floor { get; set; }
        // Когда создана запись
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
