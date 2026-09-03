using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models;
using System.Diagnostics;

namespace Renty.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(PropertyFilterViewModel filter)
        {
            var categories = new List<CategoryViewModel>
            {
                new() { Id = Guid.NewGuid(), Slug = "nice-views", Name = "Красивые виды", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "small-apartments", Name = "Небольшие квартиры", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "large-apartments", Name = "Большие квартиры", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "rooms", Name = "Комнаты", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "hostels", Name = "Хостелы", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "luxe", Name = "Люкс", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "city-center", Name = "В центре города", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "countryside", Name = "Сельская местность", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "designer", Name = "От дизайнера", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "seaside", Name = "У моря", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "mansions", Name = "Особняки", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "legendary", Name = "Легендарные", IconName = "star" },
            };

            // TEMPORARY: stands in for a real `Property` entity until EF Core is wired up —
            // holds CategorySlug so we can filter before mapping, same as a DB query would.
            var mockProperties = new List<(string CategorySlug, string ImageUrl, string City, string Country, string CategoryName, string DurationLabel, decimal Price, decimal Rating, bool IsFavorite)>
            {
                ("seaside", "https://placehold.co/600x450", "Odesa", "Ukraine", "У моря", "1-10 ночей", 70, 4.88m, false),
                ("seaside", "https://placehold.co/600x450", "Odesa", "Ukraine", "У моря", "2-7 суток", 100, 4.98m, true),
                ("seaside", "https://placehold.co/600x450", "Odesa", "Ukraine", "У моря", "25-30 суток", 75, 4.76m, false),
                ("seaside", "https://placehold.co/600x450", "Odesa", "Ukraine", "У моря", "5-11 суток", 42, 4.78m, false),
                ("seaside", "https://placehold.co/600x450", "Odesa", "Ukraine", "У моря", "5-10 дней", 30, 4.68m, false),
                ("seaside", "https://placehold.co/600x450", "Odesa", "Ukraine", "У моря", "10-20 дней", 28, 4.78m, false),
            };

            var filtered = string.IsNullOrEmpty(filter.CategorySlug)
                ? mockProperties
                : mockProperties.Where(p => p.CategorySlug == filter.CategorySlug);

            var properties = filtered.Select(p => new PropertyCardViewModel
            {
                Id = Guid.NewGuid(),
                ImageUrls = [p.ImageUrl],
                IsFavorite = p.IsFavorite,
                City = p.City,
                Country = p.Country,
                Rating = p.Rating,
                CategoryName = p.CategoryName,
                DurationLabel = p.DurationLabel,
                PricePerNight = p.Price,
            }).ToList();

            var vm = new HomeIndexViewModel
            {
                Properties = properties,
                CategoryStrip = new CategoryStripViewModel { Categories = categories, SelectedSlug = filter.CategorySlug },
                Filter = filter,
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
