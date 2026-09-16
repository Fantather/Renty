using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Queries.Property;
using Renty.Domain.Enums;
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
        private readonly IWebHostEnvironment _env;
        public PropertyCreateController(IMediator mediator, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _env = env;
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
            return RedirectToAction(nameof(PropertyCategory), new { id });
        }

        // Заглушка: PropertiesCategory — это темы для фильтра на главной ("Красивые виды",
        // "У моря" и т.п.), не тип жилья. Настоящий справочник типов жилья ещё не существует.
        [HttpGet("category/{id:guid}")]
        public async Task<IActionResult> PropertyCategory(Guid id)
        {

            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));

            // Если ошибка возвращаем на первый шаг
            if (!result.IsSuccess)
                return RedirectToAction(nameof(SavePropertyAddress));

            ViewData["PropertyId"] = id;
            var vm = new CategoryPageViewModel
            {
                Categories = new List<CategoryViewModel>
                {
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Дом" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Квартира" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Гостевой дом" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Гостиница" },
                },
                Property = new PropertyInputModel
                {
                    CategoryId = result.Data!.CategoryId!.Value
                }
            };

            return View(vm);
        }

        // TODO: сохранить CategoryId квартиры.
        [HttpPost("category/{id:guid}")]
        public async Task<IActionResult> SavePropertyCategory(Guid id, CategoryPageViewModel model)
        {
            var result = await _mediator.Send(new SavePropertyCategoryCommand(model.Property.PropertyId, CurrentUserId(), model.Property.CategoryId));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(PropertyBasics), new { id });
        }

        [HttpGet("basics/{id:guid}")]
        public async Task<IActionResult> PropertyBasics(Guid id)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));

            if (!result.IsSuccess)
                return RedirectToAction(nameof(SavePropertyAddress));

            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel
            {
                MaxGuests = result.Data!.MaxGuests!.Value,
                BathroomsCount = result.Data!.BathroomsCount!.Value,
                BedsCount = result.Data!.BedsCount!.Value,
                BedroomsCount = result.Data!.BedroomsCount!.Value
            });
        }

        // TODO: сохранить MaxGuests/BedroomsCount/BedsCount/BathroomsCount в PropertyDetails квартиры в БД.
        [HttpPost("basics/{id:guid}")]
        public async Task<IActionResult> SavePropertyBasics(Guid id, PropertyInputModel model)
        {
            var result = await _mediator.Send(new SavePropertyBasicsCommand(id, CurrentUserId(), model.MaxGuests, model.BedroomsCount, model.BedsCount, model.BathroomsCount));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }


            return RedirectToAction(nameof(PropertyAmenities), new { id });
        }

        // Заглушка: реального Query/Handler над IAmenityRepository ещё нет.
        [HttpGet("amenities/{id:guid}")]
        public async Task<IActionResult> PropertyAmenities(Guid id)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));

            if (!result.IsSuccess)
                return RedirectToAction(nameof(SavePropertyAddress));

            ViewData["PropertyId"] = id;
            var vm = new AmenitiesPageViewModel
            {
                Amenities = new List<AmenityViewModel>
                {
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000101"), Name = "Wi-Fi" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000102"), Name = "Кондиционер" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000103"), Name = "Кухня" },
                    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000104"), Name = "Стиральная машина" },
                },
                Property = new PropertyInputModel
                {
                    AmenityIds = result.Data!.AmenityIds
                }
            };

            return View(vm);
        }

        // TODO: сохранить AmenityIds квартиры.
        [HttpPost("amenities/{id:guid}")]
        public async Task<IActionResult> SavePropertyAmenities(Guid id, PropertyInputModel model)
        {
            var result = await _mediator.Send(new SavePropertyAmenitiesCommand(id,CurrentUserId(),model.AmenityIds));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ",result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(PropertyPhotos), new { id });
        }

        [HttpGet("photos/{id:guid}")]
        public async Task<IActionResult> PropertyPhotos(Guid id)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));

            if (!result.IsSuccess)
                return RedirectToAction(nameof(SavePropertyAddress));

            var images = result.Data!.Images
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new ExistingImageInputModel
            {
                Id = i.ImageId,
                ImageUrl = i.ImageUrl
            }).ToList();

            return View(new UploadPropertyImagesInputModel { 
                PropertyId = id, 
                ExistingImages = images
            });
        }

        // TODO: реально сохранить фото и создать PropertyImage для этой квартиры.
        [HttpPost("photos/{id:guid}")]
        public async Task<IActionResult> SavePropertyPhotos(Guid id, UploadPropertyImagesInputModel model)
        {

            var orderedImages = model.OrderedImages
                .Select(oi => new OrderedImageRef
                {
                    Type = oi.Type == "Existing"
                    ? OrderedImageType.Existing
                    : OrderedImageType.New,
                    Id = oi.Id,
                    FileIndex = oi.FileIndex
                }).ToList();

            var result = await _mediator.Send(new SavePropertyImagesCommand(id,CurrentUserId(), _env.WebRootPath, orderedImages ,model.Images));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));

                var resultImages = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()));
                model.ExistingImages = resultImages.Data!.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new ExistingImageInputModel
                {
                    Id = i.ImageId,
                    ImageUrl = i.ImageUrl
                }).ToList();

                return View(model);
            }

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
            return RedirectToAction(nameof(PropertyHighlights), new { id });
        }

        [HttpGet("highlights/{id:guid}")]
        public IActionResult PropertyHighlights(Guid id)
        {
            ViewData["PropertyId"] = id;
            return View(new PropertyInputModel());
        }

        // TODO: сохранить особенности квартиры (Highlights) (максимум 2).
        [HttpPost("highlights/{id:guid}")]
        public IActionResult SavePropertyHighlights(Guid id, PropertyInputModel model)
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
            return View(new PropertyInputModel());
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
