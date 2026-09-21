using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperty;
using Renty.Application.DTOs.GetTags;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record GetTagsQuery() : IRequest<OperationResult<List<TagResponse>>>;
}
