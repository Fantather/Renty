using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetUser;

namespace Renty.Application.Commands
{
    public record UpdateUserProfileCommand(Guid UserId, EditUserProfileInputDto Input) : IRequest<OperationResult<Unit>>;
}
