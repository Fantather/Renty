using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyDescriptionCommand(Guid PropertyId, Guid CurrentUserId, string Description) : IRequest<OperationResult<Guid>>;
}
