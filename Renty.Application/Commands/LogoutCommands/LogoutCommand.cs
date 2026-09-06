using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Renty.Application.Commands.LogoutCommands
{
    public record LogoutCommand(ClaimsPrincipal? Principal) : IRequest;
}
