using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record PublishPropertyCommand(Guid PropertyId, Guid CurrentUserId) : IRequest<OperationResult<Unit>>;
}
