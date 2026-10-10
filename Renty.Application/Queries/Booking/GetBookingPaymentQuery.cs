using MediatR;
using Renty.Application.Common;

namespace Renty.Application.Queries.Booking
{
    public record GetBookingPaymentQuery(Guid BookingId, Guid CurrentUserId) : IRequest<OperationResult<string>>;
}
