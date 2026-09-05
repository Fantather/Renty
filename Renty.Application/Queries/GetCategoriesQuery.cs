using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record GetCategoriesQuery() : IRequest<OperationResult<GetCategoriesResponse>>;

}
