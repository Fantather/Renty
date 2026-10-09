using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Renty.Application.Commands.ConnectedAccountCommands;
using Renty.Application.Commands.PaymentCommands;
using Renty.Infrastructure.Services.StripeAPI;
using Stripe;

namespace Renty.Web.Controllers.Api
{
    [Route("api/webhooks/stripe")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly string _webhookSecret;
        public StripeWebhookController(
            IMediator mediator,
            IOptions<StripeOptions> options)
        {
            _mediator = mediator;
            _webhookSecret = options.Value.WebhookKey;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _webhookSecret);

                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var succeeded = (PaymentIntent)stripeEvent.Data.Object;
                        await _mediator.Send(new PaymentCapturedCommand(succeeded.Id, GetBookingId(succeeded)), ct);
                        break;
                    
                    case "payment_intent.canceled":
                        var canceled = (PaymentIntent)stripeEvent.Data.Object;
                        await _mediator.Send(new PaymentCanceledCommand(canceled.Id, GetBookingId(canceled)), ct);
                        break;

                    case "payment_intent.payment_failed":
                        var failed = (PaymentIntent)stripeEvent.Data.Object;
                        await _mediator.Send(new PaymentFailedCommand(failed.Id, GetBookingId(failed)),ct);
                        break;

                    case "payment_intent.amount_capturable_updated":
                        var authorized = (PaymentIntent)stripeEvent.Data.Object;
                        await _mediator.Send(new PaymentAuthorizedCommand(authorized.Id, GetBookingId(authorized)),ct);
                        break;

                    case "charge.refunded":
                        var charge = (Charge)stripeEvent.Data.Object;
                        await _mediator.Send(new PaymentRefundedCommand(charge.PaymentIntentId,charge.Refunded),ct);
                        break;

                    case "account.updated":
                        var account = (Account)stripeEvent.Data.Object;
                        await _mediator.Send(new UpdatedConnectedAccountCommand(account.Id,account.Capabilities?.Transfers == "active"),ct);
                        break;
                }

                return Ok();
            }
            catch(StripeException ex)
            {
                return BadRequest();
            }
        }
        private static string GetBookingId(PaymentIntent pi) =>
    pi.Metadata.TryGetValue("bookingId", out var id) ? id : string.Empty;
    }
}
