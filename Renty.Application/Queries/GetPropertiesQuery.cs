using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record GetPropertiesQuery(int Page, int PageSize, string? Search, Guid? CategoryId, string? SortBy, DateTime? CheckIn, DateTime? CheckOut, int? GuestCount) : IRequest<OperationResult<GetPropertiesResponse>>;
}
