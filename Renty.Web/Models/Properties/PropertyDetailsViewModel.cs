namespace Renty.Web.Models.Properties
{
    public class PropertyDetailsViewModel
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<string> Images { get; set; } = new();

        public HostViewModel Host { get; set; } = new();

        public decimal Rating { get; set; }
        public int ReviewsCount { get; set; }
        public RatingBreakdownViewModel RatingBreakdown { get; set; } = new();

        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int MaxGuests { get; set; }
        public int Bedrooms { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }

        public List<AmenityViewModel> Amenities { get; set; } = new();
        public List<RoomViewModel> Rooms { get; set; } = new();
        public List<ReviewViewModel> Reviews { get; set; } = new();

        public string? HouseRules { get; set; }

        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = string.Empty;

        // Занятые даты — чтобы календарь бронирования знал, что заблокировать
        public List<(DateTime From, DateTime To)> BookedRanges { get; set; } = new();
    }
}
