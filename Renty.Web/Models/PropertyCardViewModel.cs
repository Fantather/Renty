namespace Renty.Web.Models
{
    public class PropertyCardViewModel
    {
        public Guid Id { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public bool IsFavorite { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string DurationLabel { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
    }
}
