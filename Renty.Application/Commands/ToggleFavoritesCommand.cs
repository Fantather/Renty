using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands
{
    public record ToggleFavoriteCommand(Guid UserId, string Slug) : IRequest<OperationResult<bool>>;
}
