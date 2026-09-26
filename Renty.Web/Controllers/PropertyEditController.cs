using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Queries;
using Renty.Application.Queries.Property;
using Renty.Domain.Enums;
using Renty.Web.Models.InputModels.Media;
using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.PropertyCreate;
using Renty.Web.Models.Shared;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Route("edit-property/{id:guid}")]
    public class PropertyEditController : Controller
    {
        private const double DefaultLatitude = 50.4501;
        private const double DefaultLongitude = 30.5234;

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PropertyEditController(
            IMediator mediator,
            IMapper mapper,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id, CancellationToken ct)
        {
            if (await LoadDraftAsync(id, ct) == null)
                return NotFound();

            return RedirectToAction(nameof(Title), new { id });
        }

        [HttpGet("address")]
        public async Task<IActionResult> Address(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(ToAddressPage(draft));
        }

        [HttpPost("address")]
        [ActionName(nameof(Address))]
        public async Task<IActionResult> SaveAddress(Guid id, AddressInputModel model)
        {
            var dto = _mapper.Map<SavePropertyAddressDto>(model);
            dto.HostId = CurrentUserId();
            dto.PropertyId = id;

            var result = await _mediator.Send(new SavePropertyAddressCommand(dto));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(Location), new { id });
        }

        [HttpGet("location")]
        public async Task<IActionResult> Location(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            SetGoogleMapsApiKey();

            return View(new LocationInputModel
            {
                Latitude = draft.Latitude ?? DefaultLatitude,
                Longitude = draft.Longitude ?? DefaultLongitude
            });
        }

        [HttpPost("location")]
        [ActionName("Location")]
        public async Task<IActionResult> SaveLocation(Guid id, LocationInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                SetGoogleMapsApiKey();
                return View(model);
            }

            var result = await _mediator.Send(new SavePropertyLocationCommand(id, CurrentUserId(), model.Latitude, model.Longitude), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                SetGoogleMapsApiKey();
                return View(model);
            }

            return RedirectToAction(nameof(Location), new { id });
        }

        [HttpGet("location-visibility")]
        public async Task<IActionResult> LocationVisibility(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            SetGoogleMapsApiKey();

            return View(ToLocationVisibilityPage(draft, new LocationVisibilityInputModel
            {
                ShowExactLocation = draft.ShowExactLocation ?? true
            }));
        }

        [HttpPost("location-visibility")]
        [ActionName("LocationVisibility")]
        public async Task<IActionResult> SaveLocationVisibility(Guid id, [Bind(Prefix = nameof(LocationVisibilityPageViewModel.Input))] LocationVisibilityInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyLocationVisibilityCommand(id, CurrentUserId(), model.ShowExactLocation), ct);

            if (!result.IsSuccess)
            {
                var draft = await LoadDraftAsync(id, ct);
                if (draft == null)
                    return NotFound();

                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                SetGoogleMapsApiKey();
                return View(ToLocationVisibilityPage(draft, model));
            }

            return RedirectToAction(nameof(LocationVisibility), new { id });
        }

        [HttpGet("category")]
        public async Task<IActionResult> Category(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new CategoryPageViewModel
            {
                Categories = await LoadCategoriesAsync(ct),
                Input = new CategoryInputModel { CategoryId = draft.CategoryId ?? Guid.Empty }
            });
        }

        [HttpPost("category")]
        [ActionName("Category")]
        public async Task<IActionResult> SaveCategory(Guid id, CategoryPageViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await LoadCategoriesAsync(ct);
                return View(model);
            }

            var result = await _mediator.Send(new SavePropertyCategoryCommand(id, CurrentUserId(), model.Input.CategoryId), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                model.Categories = await LoadCategoriesAsync(ct);
                return View(model);
            }

            return RedirectToAction(nameof(Category), new { id });
        }

        [HttpGet("basics")]
        public async Task<IActionResult> Basics(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new BasicsInputModel
            {
                MaxGuests = draft.MaxGuests ?? 1,
                BedroomsCount = draft.BedroomsCount ?? 0,
                BedsCount = draft.BedsCount ?? 0,
                BathroomsCount = draft.BathroomsCount ?? 0,
                Floor = draft.Floor ?? 0,
                FloorsCount = draft.FloorsCount ?? 1
            });
        }

        [HttpPost("basics")]
        [ActionName("Basics")]
        public async Task<IActionResult> SaveBasics(Guid id, BasicsInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var input = new PropertyBasicsInputDto
            {
                MaxGuests = model.MaxGuests,
                BedroomsCount = model.BedroomsCount,
                BedsCount = model.BedsCount,
                BathroomsCount = model.BathroomsCount,
                Floor = model.Floor,
                FloorsCount = model.FloorsCount
            };

            var result = await _mediator.Send(new SavePropertyBasicsCommand(id, CurrentUserId(), input), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(Basics), new { id });
        }

        [HttpGet("amenities")]
        public async Task<IActionResult> Amenities(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new AmenitiesPageViewModel
            {
                Amenities = await LoadAmenitiesAsync(ct),
                Input = new AmenitiesInputModel { AmenityIds = draft.AmenityIds }
            });
        }

        [HttpPost("amenities")]
        [ActionName("Amenities")]
        public async Task<IActionResult> SaveAmenities(Guid id, AmenitiesInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyAmenitiesCommand(id, CurrentUserId(), model.AmenityIds), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(new AmenitiesPageViewModel
                {
                    Amenities = await LoadAmenitiesAsync(ct),
                    Input = model
                });
            }

            return RedirectToAction(nameof(Amenities), new { id });
        }

        [HttpGet("photos")]
        public async Task<IActionResult> Photos(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new UploadPropertyImagesInputModel
            {
                ExistingImages = ToExistingImages(draft)
            });
        }

        [HttpPost("photos")]
        [ActionName("Photos")]
        public async Task<IActionResult> SavePhotos(Guid id, UploadPropertyImagesInputModel model, CancellationToken ct)
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

            var result = await _mediator.Send(new SavePropertyImagesCommand(id, CurrentUserId(), _env.WebRootPath, orderedImages, model.Images), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));

                var draft = await LoadDraftAsync(id, ct);
                if (draft == null)
                    return NotFound();

                model.ExistingImages = ToExistingImages(draft);

                return View(model);
            }

            return RedirectToAction(nameof(Photos), new { id });
        }

        [HttpGet("title")]
        public async Task<IActionResult> Title(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new TitleInputModel
            {
                Name = draft.Name ?? string.Empty
            });
        }

        [HttpPost("title")]
        [ActionName("Title")]
        public async Task<IActionResult> SaveTitle(Guid id, TitleInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(new SavePropertyTitleCommand(id, CurrentUserId(), model.Name), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(Title), new { id });
        }

        [HttpGet("tags")]
        public async Task<IActionResult> Tags(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new TagsPageViewModel
            {
                Tags = await LoadTagsAsync(ct),
                Input = new TagsInputModel { TagIds = draft.TagIds }
            });
        }

        [HttpPost("tags")]
        [ActionName("Tags")]
        public async Task<IActionResult> SaveTags(Guid id, TagsInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyTagsCommand(id, CurrentUserId(), model.TagIds), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(new TagsPageViewModel
                {
                    Tags = await LoadTagsAsync(ct),
                    Input = model
                });
            }

            return RedirectToAction(nameof(Tags), new { id });
        }

        [HttpGet("description")]
        public async Task<IActionResult> Description(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new DescriptionInputModel
            {
                Description = draft.Description ?? string.Empty
            });
        }

        [HttpPost("description")]
        [ActionName("Description")]
        public async Task<IActionResult> SaveDescription(Guid id, DescriptionInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(new SavePropertyDescriptionCommand(id, CurrentUserId(), model.Description), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(Description), new { id });
        }

        [HttpGet("booking-settings")]
        public async Task<IActionResult> BookingSettings(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new BookingSettingsInputModel
            {
                InstantBookEnabled = draft.InstantBook ?? false
            });
        }

        [HttpPost("booking-settings")]
        [ActionName("BookingSettings")]
        public async Task<IActionResult> SaveBookingSettings(Guid id, BookingSettingsInputModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new SavePropertyBookingSettingsCommand(id, CurrentUserId(), model.InstantBookEnabled), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(BookingSettings), new { id });
        }

        [HttpGet("pricing")]
        public async Task<IActionResult> Pricing(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            return View(new PricingInputModel
            {
                PricePerNight = draft.PricePerNight ?? 0,
                Currency = draft.Currency ?? new PricingInputModel().Currency,
                WeekendPricePercent = draft.WeekendPricePercent ?? 0
            });
        }

        [HttpPost("pricing")]
        [ActionName("Pricing")]
        public async Task<IActionResult> SavePricing(Guid id, PricingInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(new SavePropertyPricingCommand(id, CurrentUserId(), model.PricePerNight, model.Currency, model.WeekendPricePercent), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(Pricing), new { id });
        }

        [HttpGet("discounts")]
        public async Task<IActionResult> Discounts(Guid id, CancellationToken ct)
        {
            var draft = await LoadDraftAsync(id, ct);
            if (draft == null)
                return NotFound();

            var discounts = draft.Discounts ?? new DiscountsInputDto();

            return View(new DiscountsInputModel
            {
                MonthlyDiscountEnabled = discounts.MonthlyDiscountEnabled,
                MonthlyDiscountPercent = discounts.MonthlyDiscountPercent,
                LastMinuteDiscountEnabled = discounts.LastMinuteDiscountEnabled,
                LastMinuteDiscountPercent = discounts.LastMinuteDiscountPercent,
                NewListingDiscountEnabled = discounts.NewListingDiscountEnabled,
                NewListingDiscountPercent = discounts.NewListingDiscountPercent,
                WeeklyDiscountEnabled = discounts.WeeklyDiscountEnabled,
                WeeklyDiscountPercent = discounts.WeeklyDiscountPercent
            });
        }

        [HttpPost("discounts")]
        [ActionName("Discounts")]
        public async Task<IActionResult> SaveDiscounts(Guid id, DiscountsInputModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(new SavePropertyDiscountsCommand(id, CurrentUserId(), _mapper.Map<DiscountsInputDto>(model)), ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(model);
            }

            return RedirectToAction(nameof(Discounts), new { id });
        }

        private async Task<PropertyDraftDto?> LoadDraftAsync(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPropertyDraftQuery(id, CurrentUserId()), ct);

            return result.IsSuccess ? result.Data : null;
        }

        private async Task<List<CategoryViewModel>> LoadCategoriesAsync(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCategoriesQuery(), ct);

            if (!result.IsSuccess)
                return new List<CategoryViewModel>();

            return result.Data!.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Slug = c.Slug,
                    Name = c.Name,
                    IconName = c.IconName ?? "star"
                }).ToList();
        }

        private async Task<List<AmenityViewModel>> LoadAmenitiesAsync(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAmenitiesQuery(), ct);

            if (!result.IsSuccess)
                return new List<AmenityViewModel>();

            return result.Data!.Select(a => _mapper.Map<AmenityViewModel>(a)).ToList();
        }

        private async Task<List<TagViewModel>> LoadTagsAsync(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetTagsQuery(), ct);

            if (!result.IsSuccess)
                return new List<TagViewModel>();

            return result.Data!.Select(t => _mapper.Map<TagViewModel>(t)).ToList();
        }

        private void SetGoogleMapsApiKey()
        {
            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;
        }
        private static AddressInputModel ToAddressPage(PropertyDraftDto draft) =>
            new()
            {
                Address = draft.Address ?? string.Empty,
                District = draft.District,
                Street = draft.Street,
                CityName = draft.CityName,
                CountryName = draft.CountryName,
                PlaceId = draft.PlaceId
            };

        private static LocationVisibilityPageViewModel ToLocationVisibilityPage(PropertyDraftDto draft, LocationVisibilityInputModel input) =>
            new()
            {
                Latitude = draft.Latitude ?? DefaultLatitude,
                Longitude = draft.Longitude ?? DefaultLongitude,
                Input = input
            };

        private static List<ExistingImageInputModel> ToExistingImages(PropertyDraftDto draft) =>
            draft.Images
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new ExistingImageInputModel
                {
                    Id = i.ImageId,
                    ImageUrl = i.ImageUrl
                }).ToList();

        private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
