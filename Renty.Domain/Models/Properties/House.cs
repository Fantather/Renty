using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Orders;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Models.Media;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Модель комнаты в недвижимости для бронирования
    /// </summary>
    public class House
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Связь с Property
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        // Тип комнаты
        public Guid RoomTypeId { get; set; }
        [ForeignKey(nameof(RoomTypeId))]
        public virtual RoomType RoomType { get; set; }

        // Ценообразование
        public decimal PricePerNight { get; set; }


        // Вместимость
        public int MaxGuests { get; set; }

        public int NumberOfBedrooms { get; set; }

        public int NumberOfBeds { get; set; }

        public int NumberOfBathrooms { get; set; }

        // Площадь в квадратных метрах
        public decimal? SizeInSquareMeters { get; set; }

        // Доступность
        public bool IsAvailable { get; set; } = true;

        public int MinimumNights { get; set; } = 1;

        public int? MaximumNights { get; set; }

        // Даты создания и обновления
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
        public virtual ICollection<RoomBed> RoomBeds { get; set; } = new List<RoomBed>();
        public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    }
}
