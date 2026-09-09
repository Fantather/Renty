using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.LoginCommands
{
    public record LoginCommand(string Email, string Password, bool RememberMe = false, string? ReturnUrl = null) : IRequest<OperationResult<LoginResponse>>;
}
