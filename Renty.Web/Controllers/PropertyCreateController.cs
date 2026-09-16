using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Queries.Property;
using Renty.Domain.Models.User;
using Renty.Web.Models.InputModels.Media;
using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.PropertyCreate;
using Renty.Web.Models.Shared;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Route("create-property")]
    public class PropertyCreateController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public PropertyCreateController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }
        [HttpGet("address")]
        public IActionResult PropertyAddress()
        {
            return View(new PropertyInputModel());
        }

        // TODO: этот метод должен создавать черновик квартиры в БД и возвращать её id.
        [HttpPost("address")]
        public IActionResult SavePropertyAddress(PropertyInputModel model)
        {
            var id = Guid.NewGuid();
            return RedirectToAction(nameof(PropertyLocation), new { id });
        }

        // Заглушка: SavePropertyAddress пока не геокодит адрес (нет реального сохранения черновика),
        // поэтому карта стартует с захардкоженного центра, а не с координат введённого адреса.
        [HttpGet("location/{id:guid}")]
        public IActionResult PropertyLocation(Guid id)
        {
            ViewData["PropertyId"] = id;
            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            return View(new PropertyInputModel { Latitude = 50.4501, Longitude = 30.5234 });
        }

        // TODO: сохранить Latitude/Longitude квартиры.
        [HttpPost("location/{id:guid}")]
        public IActionResult SavePropertyLocation(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyCategory), new { id });
        }

        // Заглушка: PropertiesCategory — это темы для фильтра на главной ("Красивые виды",
        // "У моря" и т.п.), не тип жилья. Настоящий справочник типов жилья ещё не существует.
        [HttpGet("category/{id:guid}")]
        public IActionResult PropertyCategory(Guid id)
        {
            // TODO: SavePropertyAddress пока не создаёт черновик в БД (нет ключа Google Geocoding API),
            // поэтому GetPropertyDraftQuery тут всегда фейлится и рвёт визард на первом шаге.
            // Отключено, пока SavePropertyAddress не будет реально сохранять черновик.
            //var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));
            //if (!result.IsSuccess)
            //    return RedirectToAction(nameof(SavePropertyAddress));

            ViewData["PropertyId"] = id;
            var vm = new CategoryPageViewModel
            {
                Categories = new List<CategoryViewModel>
                {
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Дом", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Квартира", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Гостевой дом", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Гостиница", IconName = "star" },
                },
                Property = new PropertyInputModel()
            };

            return View(vm);
        }

        // TODO: сохранить CategoryId квартиры.
        [HttpPost("category/{id:guid}")]
        public async Task<IActionResult> SavePropertyCategory(Guid id, CategoryPageViewModel model)
        {
            //var result = await _mediator.Send(new SavePropertyCategoryCommand(model.Property.PropertyId, ))

            return RedirectToAction(nameof(PropertyBasics), new { id });
        }

        [HttpGet("basics/{id:guid}")]
        public IActionResult PropertyBasics(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: сохранить MaxGuests/BedroomsCount/BedsCount/BathroomsCount в PropertyDetails квартиры в БД.
        [HttpPost("basics/{id:guid}")]
        public IActionResult SavePropertyBasics(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyAmenities), new { id });
        }

        // Заглушка: реального Query/Handler над IAmenityRepository ещё нет.
        [HttpGet("amenities/{id:guid}")]
        public IActionResult PropertyAmenities(Guid id)
        {
            ViewData["PropertyId"] = id;
            var vm = new AmenitiesPageViewModel
            {
                Amenities = new List<AmenityViewModel>
                {
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000101"), Name = "Wi-Fi", Description = "Бесплатный Wi-Fi", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000102"), Name = "Кондиционер", Description = "Система охлаждения воздуха", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000103"), Name = "Кухня", Description = "Базовая кухня в наличии", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000104"), Name = "Стиральная машина", Description = "Стиральная машина для гостей", IconName = "star" },
                },
            };

            return View(vm);
        }

        // TODO: сохранить AmenityIds квартиры.
        [HttpPost("amenities/{id:guid}")]
        public IActionResult SavePropertyAmenities(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyPhotos), new { id });
        }

        [HttpGet("photos/{id:guid}")]
        public IActionResult PropertyPhotos(Guid id)
        {
            return View(new UploadPropertyImagesInputModel { PropertyId = id });
        }

        // TODO: реально сохранить фото и создать PropertyImage для этой квартиры.
        [HttpPost("photos/{id:guid}")]
        public IActionResult SavePropertyPhotos(Guid id, UploadPropertyImagesInputModel model)
        {
            return RedirectToAction(nameof(PropertyTitle), new { id });
        }

        [HttpGet("title/{id:guid}")]
        public IActionResult PropertyTitle(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: сохранить Name квартиры.
        [HttpPost("title/{id:guid}")]
        public IActionResult SavePropertyTitle(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyTags), new { id });
        }

        // Заглушка: реального Query/Handler над справочником Tag ещё нет.
        [HttpGet("tags/{id:guid}")]
        public IActionResult PropertyTags(Guid id)
        {
            ViewData["PropertyId"] = id;
            var vm = new TagsPageViewModel
            {
                Tags = new List<TagViewModel>
                {
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000201"), Name = "Тихое", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000202"), Name = "Уникальное", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000203"), Name = "Для семей с детьми", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000204"), Name = "Стильное", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000205"), Name = "В центре", IconName = "star" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000206"), Name = "Простор", IconName = "star" },
                },
            };

            return View(vm);
        }

        // TODO: сохранить TagIds квартиры (максимум 2).
        [HttpPost("tags/{id:guid}")]
        public IActionResult SavePropertyTags(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyDescription), new { id });
        }

        [HttpGet("description/{id:guid}")]
        public IActionResult PropertyDescription(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: сохранить Description квартиры.
        [HttpPost("description/{id:guid}")]
        public IActionResult SavePropertyDescription(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyBookingSettings), new { id });
        }

        [HttpGet("booking-settings/{id:guid}")]
        public IActionResult PropertyBookingSettings(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: сохранить InstantBookEnabled квартиры (принимаются ли заявки на бронирование автоматически).
        [HttpPost("booking-settings/{id:guid}")]
        public IActionResult SavePropertyBookingSettings(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyPricing), new { id });
        }

        [HttpGet("pricing/{id:guid}")]
        public IActionResult PropertyPricing(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel { WeekendPricePercent = 0 });
        }

        // TODO: сохранить PricePerNight/WeekendPricePercent квартиры.
        [HttpPost("pricing/{id:guid}")]
        public IActionResult SavePropertyPricing(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyDiscounts), new { id });
        }

        [HttpGet("discounts/{id:guid}")]
        public IActionResult PropertyDiscounts(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: сохранить Discounts квартиры.
        [HttpPost("discounts/{id:guid}")]
        public IActionResult SavePropertyDiscounts(Guid id, PropertyInputModel model)
        {
            return RedirectToAction(nameof(PropertyReview), new { id });
        }

        [HttpGet("review/{id:guid}")]
        public IActionResult PropertyReview(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: опубликовать объявление (сменить статус черновика) и перенаправить на страницу объекта.
        [HttpPost("review/{id:guid}")]
        public IActionResult Publish(Guid id)
        {
            return RedirectToAction("Index", "Home");
        }

        // Заглушка: показывает ожидаемую форму ответа для поиска адреса.
        [HttpGet("search-address")]
        public IActionResult SearchAddress(string searchTerm)
        {
            return Json(new[]
            {
                new { title = "Пример, Одесса", address = "ул. Примерная, 1", street = "ул. Примерная", district = "Приморский", cityId = "Одесса", countryId = "Украина" },
            });
        }

        private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
