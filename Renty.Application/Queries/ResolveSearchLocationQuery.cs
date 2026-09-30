using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Common.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record ResolveSearchLocationQuery(
        string? PlaceId,
        string? Destination
    ) : IRequest<OperationResult<SearchLocationDto>>;
}
