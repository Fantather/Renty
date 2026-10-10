using MediatR;
using Renty.Application.Common;

namespace Renty.Application.Commands.BookingCommands
{
    public record CompleteBookingPaymentCommand(Guid BookingId, Guid CurrentUserId) : IRequest<OperationResult<bool>>;
}
