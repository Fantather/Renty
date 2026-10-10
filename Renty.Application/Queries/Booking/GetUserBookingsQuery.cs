using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Booking;

namespace Renty.Application.Queries.Booking
{
    public record GetUserBookingsQuery(Guid UserId) : IRequest<OperationResult<List<UserBookingDto>>>;
}
