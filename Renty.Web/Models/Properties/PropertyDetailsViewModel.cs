using Renty.Web.Models.Shared;

namespace Renty.Web.Models.Properties
{
    // Данные для страницы деталей квартиры (Properties/Details.cshtml)
    public class PropertyDetailsViewModel
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<string> Images { get; set; } = new();

        public HostViewModel Host { get; set; } = new();

        public decimal OverallRating { get; set; } // общий рейтинг квартиры (среднее по всем отзывам)
        public int ReviewsCount { get; set; }
        public RatingBreakdownViewModel RatingBreakdown { get; set; } = new(); // средние оценки по категориям

        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; // точный адрес
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string GoogleMapsApiKey { get; set; } = string.Empty;

        public int MaxGuests { get; set; }
        public int Beds { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        public List<AmenityViewModel> Amenities { get; set; } = new(); // список удобств квартиры
        public List<RoomViewModel> Rooms { get; set; } = new();

        public List<ReviewViewModel> Reviews { get; set; } = new(); // только уже отрисованная порция — остальные подгружаются через loadMoreReviews()

        public string? HouseRules { get; set; }

        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = string.Empty;

        // Занятые даты — чтобы календарь бронирования знал, что заблокировать
        public List<(DateTime From, DateTime To)> BookedRanges { get; set; } = new();
    }
}
