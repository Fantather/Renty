using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetAmenities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record GetAmenitiesQuery() : IRequest<OperationResult<List<AmenityItem>>>;
}
