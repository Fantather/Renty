using AspNetCoreGeneratedDocument;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Queries;
using Renty.Application.Queries.Property;
using Renty.Domain.Enums;
using Renty.Domain.Models.User;
using Renty.Web.Models.InputModels.Media;
using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.PropertyCreate;
using Renty.Web.Models.Shared;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Renty.Web.Controllers
{
    [Route("create-property")]
    public class PropertyCreateController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public PropertyCreateController(
            IMediator mediator, 
            IConfiguration configuration, 
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _mediator = mediator;
            _configuration = configuration;
            _env = env;
            _mapper = mapper;
        }
        [HttpGet("address")]
        public async Task<IActionResult> PropertyAddress(AddressInputModel? model)
        {
            if (model != null)
                return View(model);

            return View(new AddressInputModel());
        }

        // Принимает айди недвижимости при возвращении на шаг назад
        [HttpGet("address/{id:guid}")]
        public async Task<IActionResult> PropertyAddress(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (result.IsSuccess)
            {
                var propertyDraft = result.Data!;
                return View(new AddressInputModel
                {
                    CityName = propertyDraft.CityName?.ToString() ?? "",
                    CountryName = propertyDraft.CountryName?.ToString() ?? "",
                    District = propertyDraft.District,
                    Street = propertyDraft.Street,
                    Address = propertyDraft.Address ?? "",
                    PlaceId = propertyDraft.PlaceId,
                });
            }
            return View(new AddressInputModel());
        }

        // TODO: этот метод должен создавать черновик квартиры в БД и возвращать её id.
        [HttpPost("address")]
        public async Task<IActionResult> SavePropertyAddress(AddressInputModel model, CancellationToken ct)
        {
            var data = new SavePropertyAddressDto
            {
                RawAddress = model.Address,
                PlaceId = model.PlaceId,
                HostId = CurrentUserId(),
            };
            var result = await _mediator.Send(new SavePropertyAddressCommand(data), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyAddress),model);
            }

            return RedirectToAction(nameof(PropertyLocation), new { id = result.Data });
        }

        // Заглушка: SavePropertyAddress пока не геокодит адрес (нет реального сохранения черновика),
        // поэтому карта стартует с захардкоженного центра, а не с координат введённого адреса.
        [HttpGet("location/{id:guid}")]
        public async Task<IActionResult> PropertyLocation(Guid id, LocationInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()), ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            if (model != null)
                return View(model);

            ViewData["PropertyId"] = id;
            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            return View(new LocationInputModel
            {
                Latitude = result.Data!.Latitude.HasValue ? result.Data.Latitude.Value : 50.4501,
                Longitude = result.Data!.Longitude.HasValue ? result.Data.Longitude.Value : 30.5234
            });
        }

        // TODO: сохранить Latitude/Longitude квартиры.
        [HttpPost("location/{id:guid}")]
        public async Task<IActionResult> SavePropertyLocation(Guid id, LocationInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyLocationCommand(id, CurrentUserId(), model.Latitude, model.Longitude), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyLocation),model);
            }

            return RedirectToAction(nameof(PropertyLocationVisibility), new { id });
        }

        [HttpGet("location-visibility/{id:guid}")]
        public async Task<IActionResult> PropertyLocationVisibility(Guid id, LocationVisibilityInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()), ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            return View(new LocationVisibilityPageViewModel
            {
                
                Input = model == null 
                ? new LocationVisibilityInputModel
                { 
                    ShowExactLocation = result.Data.ShowExactLocation.HasValue
                        ? result.Data!.ShowExactLocation!.Value
                        : true
                }
                : model,
                Latitude = result.Data!.Latitude.HasValue ? result.Data.Latitude.Value : 50.4501,
                Longitude = result.Data!.Longitude.HasValue ? result.Data.Longitude.Value : 30.5234

            });
        }

        // TODO: сохранить ShowExactLocation квартиры.
        [HttpPost("location-visibility/{id:guid}")]
        public async Task<IActionResult> SavePropertyLocationVisibility(Guid id, [Bind(Prefix = nameof(LocationVisibilityPageViewModel.Input))] LocationVisibilityInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyLocationVisibilityCommand(id, CurrentUserId(), model.ShowExactLocation), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyLocationVisibility),model);
            }

            return RedirectToAction(nameof(PropertyCategory), new { id });
        }

        // Заглушка: PropertiesCategory — это темы для фильтра на главной ("Красивые виды",
        // "У моря" и т.п.), не тип жилья. Настоящий справочник типов жилья ещё не существует.
        [HttpGet("category/{id:guid}")]
        public async Task<IActionResult> PropertyCategory(Guid id, CategoryInputModel? model, CancellationToken ct)
        {
            // TODO: SavePropertyAddress пока не создаёт черновик в БД (нет ключа Google Geocoding API),
            // поэтому GetPropertyDraftQuery тут всегда фейлится и рвёт визард на первом шаге.
            // Отключено, пока SavePropertyAddress не будет реально сохранять черновик.
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);
            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            var resultCategories = await _mediator.Send(new GetCategoriesQuery(),ct);

            var vm = new CategoryPageViewModel
            {
                //Categories = new List<CategoryViewModel>
                //{
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Дом", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Квартира", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Гостевой дом", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Гостиница", IconName = "star" },
                //},
                Categories = resultCategories.Data!.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Slug = c.Slug,
                    Name = c.Name,
                    IconName = c.IconName ?? "star"
                }).ToList(),
                Input = model == null
                ? new CategoryInputModel
                {
                    CategoryId = result.Data.CategoryId.HasValue ? result.Data.CategoryId.Value : Guid.Empty
                }
                : model
            };

            return View(vm);
        }

        // TODO: сохранить CategoryId квартиры.
        [HttpPost("category/{id:guid}")]
        public async Task<IActionResult> SavePropertyCategory(Guid id, CategoryPageViewModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyCategoryCommand(id, CurrentUserId(), model.Input.CategoryId),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyCategory),model);
            }

            return RedirectToAction(nameof(PropertyBasics), new { id });
        }

        [HttpGet("basics/{id:guid}")]
        public async Task<IActionResult> PropertyBasics(Guid id, BasicsInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));
            if (model != null)
                return View(model);

            return View(new BasicsInputModel
            {
                MaxGuests = result.Data!.MaxGuests.HasValue ? result.Data!.MaxGuests.Value : 1,
                BathroomsCount = result.Data!.BathroomsCount.HasValue ? result.Data!.BathroomsCount.Value : 0,
                BedsCount = result.Data!.BedsCount.HasValue ? result.Data!.BedsCount.Value : 0,
                BedroomsCount = result.Data!.BedroomsCount.HasValue ? result.Data!.BedroomsCount.Value : 0,
                Floor = result.Data!.Floor.HasValue ? result.Data!.Floor.Value : 0,
                FloorsCount = result.Data!.FloorsCount.HasValue 
                ? result.Data!.FloorsCount.Value 
                : 1
            });
        }

        // TODO: сохранить MaxGuests/BedroomsCount/BedsCount/BathroomsCount в PropertyDetails квартиры в БД.
        [HttpPost("basics/{id:guid}")]
        public async Task<IActionResult> SavePropertyBasics(Guid id, BasicsInputModel model, CancellationToken ct)
        {
            var input = new PropertyBasicsInputDto
            {
                BathroomsCount = model.BathroomsCount,
                BedroomsCount = model.BedroomsCount,
                MaxGuests = model.MaxGuests,
                BedsCount = model.BedsCount,
                Floor = model.Floor,
                FloorsCount = model.FloorsCount
            };
            var result = await _mediator.Send(new SavePropertyBasicsCommand(id, CurrentUserId(), input),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyBasics),model);
            }


            return RedirectToAction(nameof(PropertyAmenities), new { id });
        }

        // Заглушка: реального Query/Handler над IAmenityRepository ещё нет.
        [HttpGet("amenities/{id:guid}")]
        public async Task<IActionResult> PropertyAmenities(Guid id, AmenitiesInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            var resultAmenities = await _mediator.Send(new GetAmenitiesQuery(),ct);

            var vm = new AmenitiesPageViewModel
            {
                //Amenities = new List<AmenityViewModel>
                //{
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000101"), Name = "Wi-Fi", Description = "Бесплатный Wi-Fi", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000102"), Name = "Кондиционер", Description = "Система охлаждения воздуха", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000103"), Name = "Кухня", Description = "Базовая кухня в наличии", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000104"), Name = "Стиральная машина", Description = "Стиральная машина для гостей", IconName = "star" },
                //},
                Amenities = resultAmenities.Data!.Select(a => _mapper.Map<AmenityViewModel>(a)).ToList(),
                Input = model == null 
                ? new AmenitiesInputModel
                {
                    AmenityIds = result.Data!.AmenityIds
                }
                : model
            };

            return View(vm);
        }

        // TODO: сохранить AmenityIds квартиры.
        [HttpPost("amenities/{id:guid}")]
        public async Task<IActionResult> SavePropertyAmenities(Guid id, AmenitiesInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyAmenitiesCommand(id,CurrentUserId(),model.AmenityIds),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ",result.Errors));
                return View(nameof(PropertyAmenities),model);
            }

            return RedirectToAction(nameof(PropertyPhotos), new { id });
        }

        [HttpGet("photos/{id:guid}")]
        public async Task<IActionResult> PropertyPhotos(Guid id, UploadPropertyImagesInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            if (model != null)
                return View(model);

            var images = result.Data!.Images
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new ExistingImageInputModel
            {
                Id = i.ImageId,
                ImageUrl = i.ImageUrl
            }).ToList();

            return View(new UploadPropertyImagesInputModel {
                ExistingImages = images
            });
        }

        // TODO: реально сохранить фото и создать PropertyImage для этой квартиры.
        [HttpPost("photos/{id:guid}")]
        public async Task<IActionResult> SavePropertyPhotos(Guid id, UploadPropertyImagesInputModel model, CancellationToken ct)
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


            var result = await _mediator.Send(new SavePropertyImagesCommand(id,CurrentUserId(), _env.WebRootPath, orderedImages ,model.Images),ct);

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

                return View(nameof(PropertyPhotos),model);
            }

            return RedirectToAction(nameof(PropertyTitle), new { id });
        }

        [HttpGet("title/{id:guid}")]
        public async Task<IActionResult> PropertyTitle(Guid id, TitleInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            if (model != null)
                return View(model);

            return View(new TitleInputModel
            {
                Name = result.Data!.Name ?? string.Empty
            });
        }

        // TODO: сохранить Name квартиры.
        [HttpPost("title/{id:guid}")]
        public async Task<IActionResult> SavePropertyTitle(Guid id, TitleInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyTitleCommand(id, CurrentUserId(), model.Name),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyTitle),model);
            }

            return RedirectToAction(nameof(PropertyTags), new { id });
        }

        // Заглушка: реального Query/Handler над справочником Tag ещё нет.
        [HttpGet("tags/{id:guid}")]
        public async Task<IActionResult> PropertyTags(Guid id, TagsInputModel? model, CancellationToken ct)
        {

            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            var resultTags = await _mediator.Send(new GetTagsQuery(),ct);

            var vm = new TagsPageViewModel
            {
                //Tags = new List<TagViewModel>
                //{
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000201"), Name = "Тихое", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000202"), Name = "Уникальное", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000203"), Name = "Для семей с детьми", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000204"), Name = "Стильное", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000205"), Name = "В центре", IconName = "star" },
                //    new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000206"), Name = "Простор", IconName = "star" },
                //},
                Tags = resultTags!.Data!.Select(t => _mapper.Map<TagViewModel>(t)).ToList(),
                Input = model == null 
                ? new TagsInputModel 
                        { 
                            TagIds = result.Data!.TagIds 
                        }
                : model
            };

            return View(vm);
        }

        // TODO: сохранить TagIds квартиры (максимум 2).
        [HttpPost("tags/{id:guid}")]
        public async Task<IActionResult> SavePropertyTags(Guid id, TagsInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyTagsCommand(id, CurrentUserId(), model.TagIds),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyTags),model);
            }

            return RedirectToAction(nameof(PropertyDescription), new { id });
        }

        [HttpGet("description/{id:guid}")]
        public async Task<IActionResult> PropertyDescription(Guid id, DescriptionInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            if (model != null)
                return View(model);

            return View(new DescriptionInputModel());
        }

        // TODO: сохранить Description квартиры.
        [HttpPost("description/{id:guid}")]
        public async Task<IActionResult> SavePropertyDescription(Guid id, DescriptionInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyDescriptionCommand(id, CurrentUserId(), model.Description),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyDescription),model);
            }

            return RedirectToAction(nameof(PropertyBookingSettings), new { id });
        }

        [HttpGet("booking-settings/{id:guid}")]
        public async Task<IActionResult> PropertyBookingSettings(Guid id, BookingSettingsInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));

            if (model != null)
                return View(model);

            return View(new BookingSettingsInputModel());
        }

        // TODO: сохранить InstantBookEnabled квартиры (принимаются ли заявки на бронирование автоматически).
        [HttpPost("booking-settings/{id:guid}")]
        public async Task<IActionResult> SavePropertyBookingSettings(Guid id, BookingSettingsInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyBookingSettingsCommand(id, CurrentUserId(), model.InstantBookEnabled),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyBookingSettings),model);
            }

            return RedirectToAction(nameof(PropertyPricing), new { id });
        }

        [HttpGet("pricing/{id:guid}")]
        public async Task<IActionResult> PropertyPricing(Guid id, PricingInputModel? model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));
            if (model != null)
                return View(model);

            return View(new PricingInputModel 
            { 
                PricePerNight = result.Data!.PricePerNight ?? 0,
                Currency = result.Data!.Currency ?? string.Empty,
                WeekendPricePercent = result.Data!.WeekendPricePercent ?? 0
            });
        }

        // TODO: сохранить PricePerNight/WeekendPricePercent квартиры.
        [HttpPost("pricing/{id:guid}")]
        public async Task<IActionResult> SavePropertyPricing(Guid id, PricingInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyPricingCommand(id, CurrentUserId(), model.PricePerNight, model.Currency, model.WeekendPricePercent),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyPricing),model);
            }

            return RedirectToAction(nameof(PropertyDiscounts), new { id });
        }

        [HttpGet("discounts/{id:guid}")]
        public async Task<IActionResult> PropertyDiscounts(Guid id, DiscountsInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
                return RedirectToAction(nameof(PropertyAddress));
            if (model != null)
                return View(model);

            return View(new DiscountsInputModel
            {
                MonthlyDiscountEnabled = result.Data!.Discounts?.MonthlyDiscountEnabled ?? true,
                MonthlyDiscountPercent = result.Data!.Discounts?.MonthlyDiscountPercent ?? 0,
                LastMinuteDiscountEnabled = result.Data!.Discounts?.LastMinuteDiscountEnabled ?? true,
                LastMinuteDiscountPercent = result.Data!.Discounts?.LastMinuteDiscountPercent ?? 0,
                NewListingDiscountEnabled = result.Data!.Discounts?.NewListingDiscountEnabled ?? true,
                NewListingDiscountPercent = result.Data!.Discounts?.NewListingDiscountPercent ?? 0,
                WeeklyDiscountEnabled = result.Data!.Discounts?.WeeklyDiscountEnabled ?? true,
                WeeklyDiscountPercent = result.Data!.Discounts?.WeeklyDiscountPercent ?? 0
            });
        }

        // TODO: сохранить Discounts квартиры.
        [HttpPost("discounts/{id:guid}")]
        public async Task<IActionResult> SavePropertyDiscounts(Guid id, DiscountsInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyDiscountsCommand(id, CurrentUserId(), _mapper.Map<DiscountsInputDto>(model)),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyDiscounts),model);
            }

            return RedirectToAction(nameof(PropertyReview), new { id });
        }

        [HttpGet("review/{id:guid}")]
        public IActionResult PropertyReview(Guid id)
        {
            return View();
        }

        // TODO: опубликовать объявление (сменить статус черновика) и перенаправить на страницу объекта.
        [HttpPost("review/{id:guid}")]
        public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new PublishPropertyCommand(id, CurrentUserId()),ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(nameof(PropertyReview));
            }

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
