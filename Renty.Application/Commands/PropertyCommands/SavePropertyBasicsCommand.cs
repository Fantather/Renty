using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyBasicsCommand(Guid PropertyId, Guid CurrentUserId, int MaxGuests, int BedroomsCount, int BedsCount, int BathroomsCount) : IRequest<OperationResult<Guid>>;
}
