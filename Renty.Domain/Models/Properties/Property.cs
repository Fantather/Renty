using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Renty.Domain.Models.User;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Orders;
using Renty.Domain.Models.LookupsTables;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Модель для сдачи недвижимости в аренду (отель, дом, квартира)
    /// </summary>
    public class Property
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Владелец 
        public Guid HostId { get; set; }
        [ForeignKey(nameof(HostId))]
        public virtual ApplicationUser Host { get; set; }

        // Категория (Apartment, House, Villa, Hotel, etc.)
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual PropertiesCategory Category { get; set; }

        // Адрес и местоположение
        public string Address { get; set; } = string.Empty;

        public string? Street { get; set; }

        //область
        public string? District { get; set; }

        public Guid CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        public Guid CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        // Координаты недвижимости для отображения на карте
        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }


        // Цена
        public decimal PricePerNight { get; set; }

        public string Currency { get; set; } = "USD"; // ISO currency code


        // Рейтинг и отзывы
        public decimal AverageRating { get; set; } = 0;

        public int ReviewsCount { get; set; } = 0;



        // Правила дома (у каждой недвижимости могут быть свои хех)
        public string? HouseRules { get; set; }

        // Времена заезда/выезда
        public TimeSpan? CheckInTime { get; set; } = TimeSpan.FromHours(14); // 14:00

        public TimeSpan? CheckOutTime { get; set; } = TimeSpan.FromHours(11); // 11:00

        // Статус
        public PropertyStatusEnum Status { get; set; } = PropertyStatusEnum.Active;

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public virtual PropertyDetails Details { get; set; } = new PropertyDetails();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public virtual ICollection<PropertyTag> PropertyTags { get; set; } = new List<PropertyTag>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

        //// Вспомогательные методы для работы с характеристиками

        ///// <summary>
        ///// Получить текущие актуальные характеристики недвижимости
        ///// </summary>
        //[NotMapped]
        //public PropertyDetails? CurrentDetails => 
        //    DetailsHistory?.FirstOrDefault(d => d.ValidTo == null);

        ///// <summary>
        ///// Получить характеристики недвижимости на определенную дату
        ///// </summary>
        //public PropertyDetails? GetDetailsAt(DateTime date) =>
        //    DetailsHistory?
        //        .Where(d => d.ValidFrom <= date && (d.ValidTo == null || d.ValidTo >= date))
        //        .OrderByDescending(d => d.ValidFrom)
        //        .FirstOrDefault();
    }
}
