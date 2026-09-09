using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PasswordCommands
{
    public record ResetPasswordCommand(string UserId, string Token, string NewPassword) : IRequest<OperationResult<bool>>;
}
