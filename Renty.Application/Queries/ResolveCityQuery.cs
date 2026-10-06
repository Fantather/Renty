using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public record ResolveCityQuery(string? PlaceId, string? CityText) : IRequest<OperationResult<ResolvedCityDto>>;
}
