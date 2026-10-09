using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries.Booking
{
    public class GetBookingStatusQuery(Guid BookingId, Guid CurrentUserId) : IRequest<OperationResult<Unit>>;
}
