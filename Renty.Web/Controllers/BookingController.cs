using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Renty.Application.Commands.BookingCommands;
using Renty.Infrastructure.Services.StripeAPI;
using Renty.Web.Models.BookingModels;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;
        private readonly string _publicKey;
        public BookingController(
            IMediator mediator,
            IOptions<StripeOptions> options)
        {
            _mediator = mediator;
            _publicKey = options.Value.PkKey;
        }
        // JSON-эндпоинт, который вызывает JS
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] CreateBookingRequest body, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreateBookingCommand(CurrentUserId,body.PropertyId,body.CheckIn,body.CheckOut,body.Guests,body.PaymentMethod), ct);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Data); // { bookingId, clientSecret }
        }

        // Куда Stripe вернёт гостя после оплаты
        [HttpGet]
        public IActionResult Result(Guid bookingId) => View(bookingId);

        // Эндпоинт для опроса статуса со страницы Result
        //[HttpGet]
        //public async Task<IActionResult> Status(Guid bookingId, CancellationToken ct)
        //{
        //    var result = await _mediator.Send(new GetBookingStatusQuery(bookingId, CurrentUserId), ct);
        //    if (!result.IsSuccess)
        //        return NotFound();

        //    return Ok(result.Data); // { status, paymentStatus }
        //}
        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    }
}
