using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.LoginCommands
{
    public record GetExternalLoginsCommand() : IRequest<OperationResult<IEnumerable<ExternalLoginResponse>>>;
}
