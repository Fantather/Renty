using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.OnbordingCommands
{
    public record CreateOnboardingLinkCommand(Guid CurrentUserId, string ReturnUrl, string RefreshUrl) : IRequest<OperationResult<string>>;
}
