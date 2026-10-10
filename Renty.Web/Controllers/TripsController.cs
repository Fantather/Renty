using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries;
using Renty.Application.Queries.Booking;
using Renty.Web.Models.Shared;
using Renty.Web.Models.Trips;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TripsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var bookings = await _mediator.Send(new GetUserBookingsQuery(userId));
            var favorites = await _mediator.Send(new GetUserFavoritesQuery(userId));

            var vm = new TripsViewModel
            {
                Bookings = bookings.IsSuccess
                    ? bookings.Data!.Select(b => new PropertyCardViewModel
                    {
                        Id = b.PropertyId,
                        Slug = b.PropertySlug,
                        ActionUrl = Url.Action("Result", "Booking", new { bookingId = b.BookingId }),
                        ImageUrls = b.ImageUrls,
                        City = b.City,
                        Country = b.Country,
                        CategoryName = b.CategoryName,
                        Rating = b.Rating,
                        PricePerNight = b.PricePerNight,
                        TotalPrice = b.TotalPrice,
                        DurationLabel = b.CheckInDate.ToString("dd.MM.yyyy") + " - " + b.CheckOutDate.ToString("dd.MM.yyyy"),
                        ShowFavorite = false
                    }).ToList()
                    : new List<PropertyCardViewModel>(),
                Favorites = favorites.IsSuccess
                    ? _mapper.Map<List<PropertyCardViewModel>>(favorites.Data)
                    : new List<PropertyCardViewModel>()
            };

            return View(vm);
        }
    }
}
