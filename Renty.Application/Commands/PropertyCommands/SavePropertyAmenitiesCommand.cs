using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyAmenitiesCommand(Guid PropertyId, Guid CurrentUserId, List<Guid> Amenities) : IRequest<OperationResult<Guid>>;
}
