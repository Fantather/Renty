using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.Search;
using Renty.Web.Models.Shared;

namespace Renty.Web.Controllers
{
    public class SearchController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        public IActionResult Index(PropertyFilterViewModel filter)
        {
            var cities = new[]
            {
                ("Kyiv", "Ukraine"), ("Odesa", "Ukraine"), ("Lviv", "Ukraine"),
                ("Amsterdam", "Netherlands"), ("Rotterdam", "Netherlands"), ("Berlin", "Germany")
            };

            var cityCoordinates = new Dictionary<string, (double Lat, double Lng)>
            {
                ["Kyiv"] = (50.4501, 30.5234),
                ["Odesa"] = (46.4825, 30.7233),
                ["Lviv"] = (49.8397, 24.0297),
                ["Amsterdam"] = (52.3676, 4.9041),
                ["Rotterdam"] = (51.9244, 4.4777),
                ["Berlin"] = (52.5200, 13.4050)
            };

            var categories = new[] { "В центре города", "У моря", "Дизайнерское жильё", "Сельская местность" };

            var categoryList = new List<CategoryViewModel>
            {
                new() { Id = Guid.NewGuid(), Slug = "scenic-view", Name = "Красивые виды", IconName = "property-category/scenic-view" },
                new() { Id = Guid.NewGuid(), Slug = "apartment-small", Name = "Маленькие квартиры", IconName = "property-category/apartment-small" },
                new() { Id = Guid.NewGuid(), Slug = "apartment-large", Name = "Большие квартиры", IconName = "property-category/apartment-large" },
                new() { Id = Guid.NewGuid(), Slug = "hostel-bed", Name = "Хостелы", IconName = "property-category/hostel-bed" },
                new() { Id = Guid.NewGuid(), Slug = "luxury", Name = "Люкс", IconName = "property-category/luxury" },
                new() { Id = Guid.NewGuid(), Slug = "city-center", Name = "В центре города", IconName = "property-category/city-center" },
                new() { Id = Guid.NewGuid(), Slug = "countryside", Name = "Сельская местность", IconName = "property-category/countryside" },
                new() { Id = Guid.NewGuid(), Slug = "designer", Name = "Дизайнерское жильё", IconName = "property-category/designer" },
                new() { Id = Guid.NewGuid(), Slug = "seaside", Name = "У моря", IconName = "property-category/seaside" }
            };

            var properties = Enumerable.Range(1, 12).Select(i =>
            {
                var (city, country) = cities[i % cities.Length];
                var (lat, lng) = cityCoordinates[city];
                return new PropertyCardViewModel
                {
                    Id = Guid.NewGuid(),
                    Slug = $"mock-property-{i}",
                    ImageUrls = new List<string> { $"https://placehold.co/600x400?text=Property+{i}" },
                    IsFavorite = i % 4 == 0,
                    City = city,
                    Country = country,
                    Rating = 3.5m + (i % 15) / 10m,
                    CategoryName = categories[i % categories.Length],
                    DurationLabel = "27 сент. – 2 окт.",
                    PricePerNight = 900 + i * 37,
                    Latitude = lat + (i % 5) * 0.01,
                    Longitude = lng + (i % 5) * 0.01
                };
            }).ToList();

            return View(new SearchIndexViewModel
            {
                Properties = properties,
                CategoryStrip = new CategoryStripViewModel
                {
                    Categories = categoryList,
                    SelectedSlug = filter.CategorySlug,
                    Filter = filter
                },
                Filter = filter,
                TotalCount = properties.Count,
                GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty
            });
        }

        public IActionResult PropertiesInBounds(double north, double south, double east, double west)
        {
            return Json(new { count = 0, properties = Array.Empty<object>() });
        }
    }
}
