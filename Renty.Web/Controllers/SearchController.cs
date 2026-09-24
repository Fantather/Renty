using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries;
using Renty.Web.Models.Search;
using Renty.Web.Models.Shared;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    /// <summary>
    /// Index работает в двух режимах, их различает заголовок запроса X-Requested-With:
    /// обычный запрос (первая загрузка, кнопка «Найти», клик по категории, перезагрузка) — полная страница;
    /// запрос из search-map.js после сдвига или зума карты — только PartialView _SearchResults (заголовок, карточки, JSON пинов).
    /// Если заданы границы карты (North/South/East/West), они заменяют Destination.
    /// </summary>
    public class SearchController(IMediator mediator, IMapper mapper, IConfiguration configuration) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;

        public async Task<IActionResult> Index(PropertyFilterViewModel filter)
        {
            var currentUserId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var parsedId) ? parsedId : (Guid?)null;
            var checkIn = filter.CheckInDate?.ToDateTime(TimeOnly.MinValue);
            var checkOut = filter.CheckOutDate?.ToDateTime(TimeOnly.MaxValue);

            var hasBounds = filter.North.HasValue && filter.South.HasValue && filter.East.HasValue && filter.West.HasValue;

            var items = new List<PropertyListItem>();
            var totalCount = 0;

            var propertiesResult = await _mediator.Send(new GetPropertiesQuery
            {
                CategorySlug = filter.CategorySlug,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                Destination = hasBounds ? null : filter.Destination,
                North = filter.North,
                South = filter.South,
                East = filter.East,
                West = filter.West,
                UserId = currentUserId
            });

            if (propertiesResult.IsSuccess && propertiesResult.Data != null)
            {
                items = propertiesResult.Data.Properties;
                totalCount = propertiesResult.Data.TotalCount;
            }

            var viewModel = new SearchIndexViewModel
            {
                Properties = _mapper.Map<List<PropertyCardViewModel>>(items),
                Filter = filter,
                TotalCount = totalCount
            };

            // Запрос из search-map.js после сдвига или зума карты - отдаём только PartialView
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SearchResults", viewModel);
            }

            // Или отдаём полную модель
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
