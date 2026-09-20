using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyLocationCommand(Guid PropertyId, Guid CurrentUserId, double Latitude, double Longitude) : IRequest<OperationResult<Guid>>;
}
