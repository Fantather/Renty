using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Renty.Application.Commands.BookingCommands;
using Renty.Application.Queries.Booking;
using Renty.Infrastructure.Services.StripeAPI;
using Renty.Web.Models.BookingModels;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly string _publicKey;
        public BookingController(
            IMediator mediator,
            IMapper mapper,
            IOptions<StripeOptions> options)
        {
            _mediator = mediator;
            _mapper = mapper;
            _publicKey = options.Value.PkKey;
        }
        // JSON-эндпоинт, который вызывает JS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody] CreateBookingRequest body, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreateBookingCommand(CurrentUserId,body.PropertyId,body.CheckIn,body.CheckOut,body.Guests,body.PaymentMethod), ct);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Data); // { bookingId, clientSecret }
        }

        [HttpGet]
        public async Task<IActionResult> Pay(Guid bookingId, CancellationToken ct)
        {
            var payment = await _mediator.Send(new GetBookingPaymentQuery(bookingId, CurrentUserId), ct);
            if (!payment.IsSuccess)
                return RedirectToAction(nameof(Result), new { bookingId });

            var booking = await _mediator.Send(new GetBookingResultQuery(bookingId, CurrentUserId), ct);

            return View(new BookingPayViewModel
            {
                Booking = _mapper.Map<BookingResultViewModel>(booking.Data),
                ClientSecret = payment.Data!,
                PublishableKey = _publicKey
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletePayment(Guid bookingId, CancellationToken ct)
        {
            var result = await _mediator.Send(new CompleteBookingPaymentCommand(bookingId, CurrentUserId), ct);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok(new { redirectUrl = Url.Action(nameof(Result), new { bookingId }) });
        }

        // Куда Stripe вернёт гостя после оплаты
        [HttpGet]
        public async Task<IActionResult> Result(Guid bookingId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetBookingResultQuery(bookingId, CurrentUserId), ct);
            if (!result.IsSuccess)
                return NotFound();

            return View(_mapper.Map<BookingResultViewModel>(result.Data));
        }

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
