using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetUser;

namespace Renty.Application.Queries
{
    public record GetUserProfileQuery(
        Guid TargetUserId,
        Guid? CurrentUserId,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<OperationResult<GetUserProfileResponse>>;

}
