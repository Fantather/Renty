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
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Связь с домом
        public Guid PropertyId { get; set; }
        public virtual Property Property { get; set; }

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

        // Статус бронирования (связь с StatusLookup)
        //public int StatusId { get; set; }
        ////[ForeignKey(nameof(StatusId))]
        public BookingStatusEnum Status { get; set; } = BookingStatusEnum.Pending;
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

    }
}
