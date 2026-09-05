using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, OperationResult<GetCitiesResponse>>
    {
        private readonly ICityRepository _cityRepository;
        public GetCitiesHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        /// <summary>
        /// Находит все города чье название содержит в себе введенный текст
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetCitiesResponse>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.SearchTerm))
                return OperationResult<GetCitiesResponse>.Fail("Search term is null or empty");

            if(request.SearchTerm.Length > 2 && request.SearchTerm.Length <= 100)
                return OperationResult<GetCitiesResponse>.Fail("The limit should be from 3 to 100");

            if(request.Limit.HasValue)
                if (request.Limit.Value > 0 && request.Limit.Value < 100)
                    return OperationResult<GetCitiesResponse>.Fail("The limit should be from 1 to 100");

            int limit = request.Limit.HasValue ? request.Limit.Value : 10;

            var cities = await _cityRepository.SearchCitiesByNameAsync(request.SearchTerm, limit, ct:cancellationToken);

            // Черновой вариант маппинга
            if (cities.Any())
            {
                var citiesDto = cities.Select(c => new CityDto { CityId = c.Id, CityName = c.Name, RegionName = c.Region?.Name, CountryName = c.Country.Name }).ToList();

                return OperationResult<GetCitiesResponse>.Success(new GetCitiesResponse { Cities = citiesDto });
            }

            return OperationResult<GetCitiesResponse>.Fail("Cities not found for the search term");
        }
    }
}
