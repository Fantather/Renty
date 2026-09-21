using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetUser;

namespace Renty.Application.Queries
{
    public record GetEditUserProfileQuery(Guid UserId)
        : IRequest<OperationResult<GetEditUserProfileResponse>>;
}
