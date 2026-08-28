using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Комната в недвижимости
    /// </summary>
    public class Room
    {
        public Guid Id { get; set; }

        // Связь с недвижимостью
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }
        public Guid RoomTypeId { get; set; }

        // Название комнаты (например, "Спальня 1", "Гостиная", "Ванная")
        public string Name { get; set; } = string.Empty;

        // Описание комнаты
        public string? Description { get; set; }

        // Является ли комната общего пользования
        // true = общая (гостиная, кухня, ванная общего пользования)
        // false = приватная (спальня, личная ванная)
        public bool IsSharedSpace { get; set; } = false;

        // Количество спальных мест в этой конкретной комнате (опционально)
        // null = не применимо (например, для ванной или кухни)
        // Должно быть <= PropertyDetails.BedsCount для всего объекта
        public int? BedsCount { get; set; }

        // Площадь комнаты (опционально)
        public decimal? Area { get; set; }

        // Активна ли комната (для soft delete)
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual RoomType RoomType { get; set; }

        public virtual ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();

        public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    }
}
