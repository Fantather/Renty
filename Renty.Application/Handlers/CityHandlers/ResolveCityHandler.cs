using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.CityHandlers
{
    public class ResolveCityHandler : IRequestHandler<ResolveCityQuery, OperationResult<ResolvedCityDto>>
    {
        private readonly ILocationResolverService _locationResolverService;
        public ResolveCityHandler(
            ILocationResolverService locationResolverService)
        {
            _locationResolverService = locationResolverService;
        }
        public async Task<OperationResult<ResolvedCityDto>> Handle(ResolveCityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var city = await _locationResolverService.ResolveCityFromInputAsync(request.PlaceId, request.CityText, cancellationToken);

                var display = string.IsNullOrWhiteSpace(city.NameRu) ? city.Name : city.NameRu;
                if(city.Country != null)
                {
                    var countryName = string.IsNullOrWhiteSpace(city.Country.NameRu) ? city.Country.Name : city.Country.NameRu;
                    display += $", {countryName}";
                }

                return OperationResult<ResolvedCityDto>.Success(new ResolvedCityDto
                {
                    CityId = city.Id,
                    DisplayName = display
                });
            }
            catch(Exception ex)
            {
                return OperationResult<ResolvedCityDto>.Fail(ex.Message);
            }
            
        }
    }
}
