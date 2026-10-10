namespace Renty.Web.Models.BookingModels
{
    public class BookingResultViewModel
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
        public string Status { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
    }
}
