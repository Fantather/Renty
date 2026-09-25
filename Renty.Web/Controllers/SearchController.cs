using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries;
using Renty.Application.Queries.Property;
using Renty.Web.Models.Search;
using Renty.Web.Models.Shared;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SearchController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Index работает в двух режимах, их различает заголовок запроса X-Requested-With:
        /// обычный запрос (первая загрузка, кнопка «Найти», клик по категории, перезагрузка) — полная страница;
        /// запрос из search-map.js после сдвига или зума карты — только PartialView _SearchResults (заголовок, карточки, JSON пинов).
        /// Если заданы границы карты (North/South/East/West), они заменяют Destination.
        /// </summary>
        public async Task<IActionResult> Index(PropertyFilterViewModel filter)
        {
            var currentUserId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var parsedId) ? parsedId : (Guid?)null;
            var checkIn = filter.CheckInDate?.ToDateTime(TimeOnly.MinValue);
            var checkOut = filter.CheckOutDate?.ToDateTime(TimeOnly.MaxValue);

            // Проверяем, есть ли координаты от карты
            var hasBounds = filter.North.HasValue && filter.South.HasValue && filter.East.HasValue && filter.West.HasValue;

            var items = new List<PropertyCardViewModel>();
            var totalCount = 0;

            if (hasBounds)
            {
                // Поиск по границам карты
                var mapResult = await _mediator.Send(new GetPropertiesByMapQuery(
                    North: filter.North!.Value,
                    South: filter.South!.Value,
                    East: filter.East!.Value,
                    West: filter.West!.Value,
                    CategorySlug: filter.CategorySlug,
                    CheckInDate: checkIn,
                    CheckOutDate: checkOut,
                    PetsAllowed: filter.Pets,
                    GuestCount: filter.GuestCount,
                    UserId: currentUserId
                ));

                if (mapResult.IsSuccess && mapResult.Data != null)
                {
                    items = _mapper.Map<List<PropertyCardViewModel>>(mapResult.Data.Properties);
                    totalCount = mapResult.Data.TotalCount;
                }
            }
            else
            {
                // Стандартный поиск по городу
                var propertiesResult = await _mediator.Send(new GetPropertiesQuery
                {
                    CategorySlug = filter.CategorySlug,
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    Destination = filter.Destination,
                    GuestCount = filter.GuestCount,
                    PetsAllowed = filter.Pets,
                    UserId = currentUserId,
                });

                if (propertiesResult.IsSuccess && propertiesResult.Data != null)
                {
                    items = _mapper.Map<List<PropertyCardViewModel>>(propertiesResult.Data.Properties);
                    totalCount = propertiesResult.Data.TotalCount;
                }
            }

            var viewModel = new SearchIndexViewModel
            {
                Properties = items,
                Filter = filter,
                TotalCount = totalCount
            };

            // Запрос из search-map.js после сдвига или зума карты - отдаём только PartialView
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SearchResults", viewModel);
            }

            // Запрос при первой загрузке страницы - отдаём полную модель с категориями
            var categoryList = new List<CategoryViewModel>();
            var categoriesResult = await _mediator.Send(new GetCategoriesQuery());

            if (categoriesResult.IsSuccess && categoriesResult.Data != null)
            {
                categoryList = _mapper.Map<List<CategoryViewModel>>(categoriesResult.Data.Categories);
            }

            viewModel.CategoryStrip = new CategoryStripViewModel

            {
                Categories = categoryList,

                SelectedSlug = filter.CategorySlug,
                Filter = filter
            };
            viewModel.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            return View(viewModel);
        }
    }
}
