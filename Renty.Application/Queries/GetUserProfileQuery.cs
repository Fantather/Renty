using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetUser;

namespace Renty.Application.Queries
{
    public record GetUserProfileQuery(Guid TargetUserId, Guid? CurrentUserId = null)
        : IRequest<OperationResult<GetUserProfileResponse>>;
}
