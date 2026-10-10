namespace Renty.Web.Models.BookingModels
{
    public class BookingPayViewModel
    {
        public BookingResultViewModel Booking { get; set; } = new();
        public string ClientSecret { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
    }
}
