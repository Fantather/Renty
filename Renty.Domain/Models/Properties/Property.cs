using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.User;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Models.Media;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Модель для сдачи недвижимости в аренду (отель, дом, квартира)
    /// </summary>
    public class Property
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Владелец 
        public long HostId { get; set; }
        [ForeignKey(nameof(HostId))]
        public virtual ApplicationUser Host { get; set; }

        // Категория (Apartment, House, Villa, Hotel, etc.)
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual ApartmentsCategory Category { get; set; }

        // Адрес и местоположение
        public string Address { get; set; } = string.Empty;

        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        // Рейтинг и отзывы
        public decimal AverageRating { get; set; } = 0;

        public int ReviewsCount { get; set; } = 0;


        // Правила дома (у каждой недвижимости могут быть свои хех)
        public string? HouseRules { get; set; }

        // Времена заезда/выезда
        public TimeSpan? CheckInTime { get; set; } = TimeSpan.FromHours(14); // 14:00

        public TimeSpan? CheckOutTime { get; set; } = TimeSpan.FromHours(11); // 11:00

        // Статус
        public bool IsActive { get; set; } = true;

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual ICollection<House> Rooms { get; set; } = new List<House>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    }
}
