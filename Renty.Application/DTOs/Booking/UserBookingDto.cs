namespace Renty.Application.DTOs.Booking
{
    public class UserBookingDto
    {
        public Guid BookingId { get; set; }
        public Guid PropertyId { get; set; }
        public string PropertySlug { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
