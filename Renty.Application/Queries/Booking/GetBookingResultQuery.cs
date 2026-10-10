using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Booking;

namespace Renty.Application.Queries.Booking
{
    public record GetBookingResultQuery(Guid BookingId, Guid CurrentUserId) : IRequest<OperationResult<BookingResultDto>>;
}
