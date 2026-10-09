using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.ConnectedAccountCommands
{
    public record UpdatedConnectedAccountCommand(string ConnectedAccountId, bool CanReceiveTransfers) : IRequest<Unit>;
}
