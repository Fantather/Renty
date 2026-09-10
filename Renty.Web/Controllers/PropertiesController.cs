using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.Properties;

namespace Renty.Web.Controllers
{
    public class PropertiesController : Controller
    {
        private const decimal MockPricePerNight = 63m;

        private readonly IConfiguration _configuration;

        public PropertiesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // TEMPORARY: заглушка вместо реального GetPropertyDetailsQuery — контроллер/DI на бэке ещё не готовы.
        // Когда будут готовы, тело метода заменится на вызов _mediator.Send(new GetPropertyDetailsQuery(slug, userId)),
        // а ViewModel и Details.cshtml трогать не придётся.
        public IActionResult Details(string slug)
        {
            var vm = new PropertyDetailsViewModel
            {
                Slug = slug,
                GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty,
                Name = "Студия и спальня с панорамой на город! Возле моря!",
                Description = "На краю моря, в тихом и уютном районе, мы предлагаем эту прекрасную студию площадью 45 м², недавно отремонтированную.\n\n" +
                              "Идеально подходит для пар, которые ищут идеальное место для знакомства с городом.\n\n" +
                              "5 минут пешком до пляжа, 10 минут до центра города.",
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
                    FullName = "Илона",
                    AvatarUrl = "https://placehold.co/80x80",
                    IsSuperhost = true,
                    ResponseSpeed = "Отвечает в течение часа",
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
                City = "Одесса",
                Country = "Украина",
                Address = "Одесса, Одесская область, Украина",
                Latitude = 46.4825,
                Longitude = 30.7233,
                MaxGuests = 4,
                Beds = 2,
                Bedrooms = 1,
                Bathrooms = 1,
                Amenities = new List<AmenityViewModel>
                {
                    new() { Name = "Отдельное рабочее место", IconName = "star" },
                    new() { Name = "Wi-Fi", IconName = "star" },
                    new() { Name = "Кухня", IconName = "star" },
                    new() { Name = "Телевизор", IconName = "star" },
                    new() { Name = "Стиральная машина", IconName = "star" },
                    new() { Name = "Вид на море", IconName = "star" },
                    new() { Name = "Лифт", IconName = "star" },
                    new() { Name = "Кондиционер", IconName = "star" },
                    new() { Name = "Бесплатная парковка", IconName = "star" },
                },
                Rooms = new List<RoomViewModel>
                {
                    new() { Name = "Спальня", RoomTypeId = RoomViewModel.BedroomTypeId, ImageUrl = "https://placehold.co/500x400" },
                    new() { Name = "Гостиная", RoomTypeId = Guid.NewGuid(), ImageUrl = "https://placehold.co/500x400" },
                    new() { Name = "Ванная комната", RoomTypeId = RoomViewModel.BathroomTypeId, ImageUrl = "https://placehold.co/500x400" },
                },
                Reviews = new List<ReviewViewModel>
                {
                    new() { AuthorName = "Stefan", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Отличное расположение, быстрый доступ к транспорту, магазины рядом.", CreatedAt = DateTime.UtcNow.AddDays(-14) },
                    new() { AuthorName = "Giulia", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Очень понравилось расположение, квартира просторная и современная.", CreatedAt = DateTime.UtcNow.AddDays(-14) },
                    new() { AuthorName = "Maeva", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Красиво, очень чисто и функционально!", CreatedAt = DateTime.UtcNow.AddDays(-21) },
                    new() { AuthorName = "Sjors", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Отличное пребывание. Чистота и прекрасное расположение.", CreatedAt = DateTime.UtcNow.AddDays(-21) },
                },
                HouseRules = "Заезд после 15:00\nВыезд до 11:00\nМаксимум 4 гостя",
                PricePerNight = MockPricePerNight,
                Currency = "USD",
                BookedRanges = new List<(DateTime From, DateTime To)>
                {
                    (new DateTime(2026, 9, 20), new DateTime(2026, 9, 25)),
                    (new DateTime(2026, 10, 3), new DateTime(2026, 10, 9)),
                },
            };

            return View(vm);
        }

        public IActionResult Price(string slug, DateOnly checkIn, DateOnly checkOut)
        {
            var nights = checkOut.DayNumber - checkIn.DayNumber;
            if (nights <= 0)
            {
                return BadRequest();
            }

            var total = nights * MockPricePerNight;

            return Json(new { nights, pricePerNight = MockPricePerNight, total });
        }
    }
}
