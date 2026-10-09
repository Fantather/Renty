using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.StripeAccountCommands
{
    public record CreateConnectedAccountCommand(Guid CurrentUserId) : IRequest<OperationResult<Unit>>;
}
