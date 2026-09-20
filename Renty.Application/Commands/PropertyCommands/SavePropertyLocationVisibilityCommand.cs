using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyLocationVisibilityCommand(Guid PropertyId, Guid CurrentUserId, bool ShowExactLocation) : IRequest<OperationResult<Guid>>;
}
