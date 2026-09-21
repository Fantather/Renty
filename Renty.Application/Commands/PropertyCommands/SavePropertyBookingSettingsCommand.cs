using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyBookingSettingsCommand(Guid PropertyId, Guid CurrentUserId, bool InstantBook) : IRequest<OperationResult<Guid>>;
}
