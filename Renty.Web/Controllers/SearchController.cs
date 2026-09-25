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

            var viewModel = new SearchIndexViewModel
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
            };

            // search-map.js после сдвига или зума карты ждёт только фрагмент, а не целую страницу
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SearchResults", viewModel);
            }

            return View(viewModel);
        }

        public IActionResult PropertiesInBounds(double north, double south, double east, double west)
        {
            return Json(new { count = 0, properties = Array.Empty<object>() });
        }

        // TODO (Ольга): ниже закомментирована рабочая версия Index с реальными данными. Она работала на странице поиска до отката,
        // а сейчас вместо неё выше мок (12 фейковых квартир), чтобы не менять бэкенд.
        // Чтобы вернуть реальные данные:
        // 1) конструктор заменить на SearchController(IMediator mediator, IMapper mapper, IConfiguration configuration) и добавить поля _mediator и _mapper;
        // 2) добавить using: AutoMapper, MediatR, System.Security.Claims, Renty.Application.DTOs.GetProperties,
        //    Renty.Application.Queries и Renty.Application.Queries.Property;
        // 3) мок Index и заглушку PropertiesInBounds удалить, раскомментировать метод ниже (JS заглушку не вызывает);
        // 4) перед этим сделать TODO из GetPropertiesHandler (TotalCount) и GetPropertiesByMapHandler (Duration, сортировка,
        //    бронирования и т. д.), иначе заголовок «N вариантов жилья» будет 0, а у карточек после сдвига карты пропадут даты.
        // Логика метода: если заданы North/South/East/West, отправляется GetPropertiesByMapQuery (без Destination), иначе GetPropertiesQuery.
        // Запрос из search-map.js (заголовок X-Requested-With) должен получать PartialView("_SearchResults"), а не целую страницу.
        //
        // /// <summary>
        // /// Index работает в двух режимах, их различает заголовок запроса X-Requested-With:
        // /// обычный запрос (первая загрузка, кнопка «Найти», клик по категории, перезагрузка) — полная страница;
        // /// запрос из search-map.js после сдвига или зума карты — только PartialView _SearchResults (заголовок, карточки, JSON пинов).
        // /// Если заданы границы карты (North/South/East/West), они заменяют Destination.
        // /// </summary>
        // public async Task<IActionResult> Index(PropertyFilterViewModel filter)
        // {
        //     var currentUserId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var parsedId) ? parsedId : (Guid?)null;
        //     var checkIn = filter.CheckInDate?.ToDateTime(TimeOnly.MinValue);
        //     var checkOut = filter.CheckOutDate?.ToDateTime(TimeOnly.MaxValue);
        //
        //     var hasBounds = filter.North.HasValue && filter.South.HasValue && filter.East.HasValue && filter.West.HasValue;
        //
        //     var items = new List<PropertyListItem>();
        //     var totalCount = 0;
        //
        //     if (hasBounds)
        //     {
        //         var mapResult = await _mediator.Send(new GetPropertiesByMapQuery(
        //             North: filter.North!.Value,
        //             South: filter.South!.Value,
        //             East: filter.East!.Value,
        //             West: filter.West!.Value,
        //             CategorySlug: filter.CategorySlug,
        //             CheckInDate: checkIn,
        //             CheckOutDate: checkOut,
        //             UserId: currentUserId));
        //
        //         if (mapResult.IsSuccess && mapResult.Data != null)
        //         {
        //             items = mapResult.Data.Properties;
        //             totalCount = mapResult.Data.TotalCount;
        //         }
        //     }
        //     else
        //     {
        //         var propertiesResult = await _mediator.Send(new GetPropertiesQuery
        //         {
        //             CategorySlug = filter.CategorySlug,
        //             CheckInDate = checkIn,
        //             CheckOutDate = checkOut,
        //             Destination = filter.Destination,
        //             UserId = currentUserId
        //         });
        //
        //         if (propertiesResult.IsSuccess && propertiesResult.Data != null)
        //         {
        //             items = propertiesResult.Data.Properties;
        //             totalCount = propertiesResult.Data.TotalCount;
        //         }
        //     }
        //
        //     var viewModel = new SearchIndexViewModel
        //     {
        //         Properties = _mapper.Map<List<PropertyCardViewModel>>(items),
        //         Filter = filter,
        //         TotalCount = totalCount
        //     };
        //
        //     // Запрос из search-map.js после сдвига или зума карты - отдаём только PartialView
        //     if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //     {
        //         return PartialView("_SearchResults", viewModel);
        //     }
        //
        //     // Или отдаём полную модель
        //     var categoryList = new List<CategoryViewModel>();
        //     var categoriesResult = await _mediator.Send(new GetCategoriesQuery());
        //
        //     if (categoriesResult.IsSuccess && categoriesResult.Data != null)
        //     {
        //         categoryList = _mapper.Map<List<CategoryViewModel>>(categoriesResult.Data.Categories);
        //     }
        //
        //     viewModel.CategoryStrip = new CategoryStripViewModel
        //     {
        //         Categories = categoryList,
        //         SelectedSlug = filter.CategorySlug,
        //         Filter = filter
        //     };
        //     viewModel.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;
        //
        //     return View(viewModel);
        // }
    }
}
