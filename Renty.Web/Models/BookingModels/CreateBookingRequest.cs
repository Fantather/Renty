using Renty.Domain.Enums;

namespace Renty.Web.Models.BookingModels
{
    public class CreateBookingRequest
    {
        public Guid PropertyId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
    }
}
