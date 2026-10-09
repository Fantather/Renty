using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetReviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record GetHostReviewsQuery(Guid HostId, int Page = 1, int PageSize = 10) : IRequest<OperationResult<GetReviewsResponse>>;
}
