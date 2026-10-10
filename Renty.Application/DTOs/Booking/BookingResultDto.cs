using Renty.Domain.Models.LookupsTables;

namespace Renty.Application.DTOs.Booking
{
    public class BookingResultDto
    {
        public Guid BookingId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertySlug { get; set; } = string.Empty;
        public string? PropertyImageUrl { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = string.Empty;
        public BookingStatusEnum Status { get; set; }
    }
}
