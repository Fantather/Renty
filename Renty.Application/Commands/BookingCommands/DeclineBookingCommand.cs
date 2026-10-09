using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.BookingCommands
{
    public record DeclineBookingCommand(Guid BookingId, Guid CurrentUserId) : IRequest<OperationResult<Unit>>;
}
