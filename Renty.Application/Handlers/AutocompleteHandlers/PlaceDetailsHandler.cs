using MediatR;
using Renty.Application.Common;
using Renty.Application.Queries.Autocomplete;
using Renty.Domain.Interfaces;
using Renty.Domain.ServiceModels.Places;

namespace Renty.Application.Handlers.AutocompleteHandlers
{
    public class PlaceDetailsHandler : IRequestHandler<PlaceDetailsQuery, OperationResult<PlaceAddressDto>>
    {
        private readonly IGoogleGeocodingService _geocodingService;
        public PlaceDetailsHandler(IGoogleGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;
        }
        public async Task<OperationResult<PlaceAddressDto>> Handle(PlaceDetailsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.PlaceId))
                return OperationResult<PlaceAddressDto>.Fail("Не указан идентификатор места");

            var details = await _geocodingService.GetAddressDetailsByPlaceIdAsync(request.PlaceId, cancellationToken);

            if (details == null)
                return OperationResult<PlaceAddressDto>.Fail("Не удалось получить данные адреса");

            return OperationResult<PlaceAddressDto>.Success(new PlaceAddressDto
            {
                StreetName = details.StreetName,
                StreetNumber = details.StreetNumber,
                District = details.DistrictName,
                CityName = details.CityName,
                CountryName = details.CountryName
            });
        }
    }
}
