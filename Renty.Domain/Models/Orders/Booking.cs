using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.User;
namespace Renty.Domain.Models.Orders
{
    /// <summary>
    /// Модель для бронирования недвижимости 
    /// </summary>
    public class Booking
    {
        public Guid Id { get; set; }

        // Связь с домом
        public Guid HouseId { get; set; }
        //[ForeignKey(nameof(HouseId))]
        public virtual House House { get; set; }

        // тот кто заезжает
        public Guid UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        // Даты заезда и выезда
        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        // Количество гостей 
        public int GuestsCount { get; set; }

        // Стоимость
        public decimal TotalPrice { get; set; }

        // Статус бронирования (связь с StatusLookup, Category="Booking")
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual BookingStatus Status { get; set; }

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

    }
}
