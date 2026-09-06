using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.Properties;

namespace Renty.Web.Controllers
{
    public class PropertiesController : Controller
    {
        // TEMPORARY: заглушка вместо реального GetPropertyDetailsQuery — контроллер/DI на бэке ещё не готовы.
        // Когда будут готовы, тело метода заменится на вызов _mediator.Send(new GetPropertyDetailsQuery(slug, userId)),
        // а ViewModel и Details.cshtml трогать не придётся.
        public IActionResult Details(string slug)
        {
            var vm = new PropertyDetailsViewModel
            {
                Slug = slug,
                Name = "Студія та спальня з панорамою на місто! Біля моря!",
                Description = "На краю моря, в тихому і затишному районі, ми пропонуємо цю чудову студію площею 45 м², нещодавно відремонтовану.\n\n" +
                              "Ідеально підходить для пар, які шукають ідеальне місце для знайомства з містом.\n\n" +
                              "5 хвилин пішки до пляжу, 10 хвилин до центру міста.",
                Images = new List<string>
                {
                    "https://placehold.co/1200x900",
                    "https://placehold.co/600x450",
                    "https://placehold.co/600x450",
                    "https://placehold.co/600x450",
                    "https://placehold.co/600x450",
                },
                Host = new HostViewModel
                {
                    FullName = "Ілона",
                    AvatarUrl = "https://placehold.co/80x80",
                    IsSuperhost = true,
                    ResponseSpeed = "Відповідає протягом години",
                    YearsHosting = 3,
                },
                OverallRating = 4.95m,
                ReviewsCount = 35,
                RatingBreakdown = new RatingBreakdownViewModel
                {
                    Cleanliness = 4.9m,
                    Accuracy = 4.9m,
                    CheckIn = 4.8m,
                    Communication = 4.9m,
                    Location = 4.8m,
                    Value = 4.8m,
                },
                City = "Одеса",
                Country = "Україна",
                Address = "Одеса, Одеська область, Україна",
                Latitude = 46.4825,
                Longitude = 30.7233,
                MaxGuests = 4,
                Beds = 2,
                Amenities = new List<AmenityViewModel>
                {
                    new() { Name = "Окреме робоче місце", IconName = "star" },
                    new() { Name = "Wi-Fi", IconName = "star" },
                    new() { Name = "Кухня", IconName = "star" },
                    new() { Name = "Телевізор", IconName = "star" },
                    new() { Name = "Пральна машина", IconName = "star" },
                    new() { Name = "Вид на море", IconName = "star" },
                    new() { Name = "Ліфт", IconName = "star" },
                    new() { Name = "Кондиціонер", IconName = "star" },
                    new() { Name = "Безкоштовна парковка", IconName = "star" },
                },
                Rooms = new List<RoomViewModel>
                {
                    new() { Name = "Спальня", RoomType = "Спальня", Description = "1 ліжко queen-size", ImageUrl = "https://placehold.co/500x400" },
                    new() { Name = "Вітальня", RoomType = "Гостиная", Description = "1 диван-ліжко", ImageUrl = "https://placehold.co/500x400" },
                    new() { Name = "Ванна кімната", RoomType = "Ванная комната", Description = "Душова кабіна", ImageUrl = "https://placehold.co/500x400" },
                },
                Reviews = new List<ReviewViewModel>
                {
                    new() { AuthorName = "Stefan", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Відмінне розташування, швидкий доступ до транспорту, магазини поруч.", CreatedAt = DateTime.UtcNow.AddDays(-14) },
                    new() { AuthorName = "Giulia", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Дуже сподобалось розташування, квартира простора і сучасна.", CreatedAt = DateTime.UtcNow.AddDays(-14) },
                    new() { AuthorName = "Maeva", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Красиво, дуже чисто і функціонально!", CreatedAt = DateTime.UtcNow.AddDays(-21) },
                    new() { AuthorName = "Sjors", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Відмінне перебування. Чистота і прекрасне розташування.", CreatedAt = DateTime.UtcNow.AddDays(-21) },
                },
                HouseRules = "Прибуття після 15:00\nВиїзд до 11:00\nМаксимум 4 гості",
                PricePerNight = 63,
                Currency = "USD",
                BookedRanges = new List<(DateTime From, DateTime To)>
                {
                    (new DateTime(2026, 9, 20), new DateTime(2026, 9, 25)),
                    (new DateTime(2026, 10, 3), new DateTime(2026, 10, 9)),
                },
            };

            return View(vm);
        }
    }
}
