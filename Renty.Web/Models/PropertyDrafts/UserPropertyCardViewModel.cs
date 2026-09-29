namespace Renty.Web.Models.PropertyDrafts
{
    public class UserPropertyCardViewModel
    {
        public Guid Id { get; set; }
        public string? ActionUrl { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
