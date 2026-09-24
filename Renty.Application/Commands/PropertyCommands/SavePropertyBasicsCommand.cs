using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyBasicsCommand(Guid PropertyId, Guid CurrentUserId, PropertyBasicsInputDto input) : IRequest<OperationResult<Guid>>;
}
