using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyCategoryCommand(Guid PropertyId, Guid CurrentUserId, Guid CategoryId) : IRequest<OperationResult<Guid>>;

}
