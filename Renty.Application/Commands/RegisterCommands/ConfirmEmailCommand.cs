using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.RegisterCommands
{
    public record ConfirmEmailCommand(string UserId, string Token) : IRequest<OperationResult<bool>>;
}
