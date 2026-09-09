using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PasswordCommands
{
    public record ForgotPasswordCommand(string Email, string ResetPasswordBaseUrl) : IRequest<OperationResult<bool>>;
}
