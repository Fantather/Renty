using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.RegisterCommands
{
    public record RegisterCommand(string UserName, string Email, string Password, string ConfirmEmailBaseUrl) : IRequest<OperationResult<bool>>;
}
