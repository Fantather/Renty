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
                new() { Id = Guid.NewGuid(), Slug = "nice-views", Name = "Гарні краєвиди", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "small-apartments", Name = "Невеликі квартири", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "large-apartments", Name = "Великі квартири", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "rooms", Name = "Кімнати", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "hostels", Name = "Хостели", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "luxe", Name = "Luxe", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "city-center", Name = "У центрі міста", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "countryside", Name = "Сільська місцевість", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "designer", Name = "Від дизайнера", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "seaside", Name = "Біля моря", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "mansions", Name = "Особняки", IconName = "star" },
                new() { Id = Guid.NewGuid(), Slug = "legendary", Name = "Легендарне", IconName = "star" },
            };

            // TEMPORARY: stands in for a real `Property` entity until EF Core is wired up —
            // holds CategorySlug so we can filter before mapping, same as a DB query would.
            var mockProperties = new List<(string CategorySlug, string ImageUrl, string City, string Country, string CategoryName, string DurationLabel, decimal Price, decimal Rating, bool IsFavorite)>
            {
                ("seaside", "https://picsum.photos/seed/renty1/600/450", "Odesa", "Ukraine", "Біля моря", "1-10 ночей", 70, 4.88m, false),
                ("seaside", "https://picsum.photos/seed/renty2/600/450", "Odesa", "Ukraine", "Біля моря", "2-7 діб", 100, 4.98m, true),
                ("seaside", "https://picsum.photos/seed/renty3/600/450", "Odesa", "Ukraine", "Біля моря", "25-30 руб", 75, 4.76m, false),
                ("seaside", "https://picsum.photos/seed/renty4/600/450", "Odesa", "Ukraine", "Біля моря", "5-11 руб", 42, 4.78m, false),
                ("seaside", "https://picsum.photos/seed/renty5/600/450", "Odesa", "Ukraine", "Біля моря", "5-10 днів", 30, 4.68m, false),
                ("seaside", "https://picsum.photos/seed/renty6/600/450", "Odesa", "Ukraine", "Біля моря", "10-20 днів", 28, 4.78m, false),
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
