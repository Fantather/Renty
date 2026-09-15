using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Renty.Application.Queries;
using Renty.Web.Models.Properties;

namespace Renty.Web.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PropertiesController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("properties/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return BadRequest();
            }

            // айди пользователя, если он авторизован, иначе null
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? userId = Guid.TryParse(userIdClaim, out var parsedId) ? parsedId : null;

            // запрос 
            var query = new GetPropertyDetailsQuery(slug, userId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }

            // мапинг
            var vm = _mapper.Map<PropertyDetailsViewModel>(result.Data);

            // апи ключ кар
            vm.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            return View(vm);
        }

        [HttpGet("properties/{slug}/price")]
        public async Task<IActionResult> Price(string slug, DateOnly checkIn, DateOnly checkOut)
        {
            var nights = checkOut.DayNumber - checkIn.DayNumber;
            if (nights <= 0)
            {
                return BadRequest("Некорректные даты бронирования.");
            }

            // TODO: Заменить на вызов MediatR, когда будет готов обработчик расчета скидок
            // var query = new CalculatePriceQuery { Slug = slug, CheckIn = checkIn, CheckOut = checkOut };
            // var result = await _mediator.Send(query);
            // return Json(result.Data);

            // Временная заглушка, чтобы не ломать фронтенд до реализации логики скидок
            var mockPricePerNight = 63m;
            var total = nights * mockPricePerNight;

            return Json(new { nights, pricePerNight = mockPricePerNight, total });
        }
    }
}