using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using AutoMapper;

namespace Renty.Application.Handlers
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, OperationResult<GetCitiesResponse>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        public GetCitiesHandler(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
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

            if (request.Limit.HasValue && (request.Limit.Value <= 0 || request.Limit.Value > 100))
                return OperationResult<GetCitiesResponse>.Fail("The limit should be from 1 to 100");

            int limit = request.Limit ?? 10;

            // Запрашиваем города из БД
            var cities = await _cityRepository.SearchCitiesByNameAsync(request.SearchTerm, limit, ct: cancellationToken);

            if (cities.Any())
            {
                var citiesDto = _mapper.Map<List<CityDto>>(cities);

                return OperationResult<GetCitiesResponse>.Success(new GetCitiesResponse { Cities = citiesDto });
            }

            return OperationResult<GetCitiesResponse>.Fail("Cities not found for the search term");
        }
    }
}
