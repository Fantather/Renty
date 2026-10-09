using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record GetUserFavoritesQuery(Guid UserId) : IRequest<OperationResult<List<PropertyListItem>>>;
}
