using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Common.Search;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Domain.ServiceModels.Locations;

namespace Renty.Application.Handlers
{
    public class LocationHandler : IRequestHandler<ResolveSearchLocationQuery, OperationResult<SearchLocationDto>>
    {
        private readonly IGoogleGeocodingService _googleGeocodingService;
        public LocationHandler(IGoogleGeocodingService googleGeocodingService)
        {
            _googleGeocodingService = googleGeocodingService;
        }

        public async Task<OperationResult<SearchLocationDto>> Handle(ResolveSearchLocationQuery request, CancellationToken cancellationToken)
        {
            AddressDetailsDto? result = null;
            if (!string.IsNullOrEmpty(request.PlaceId))
            {
                result = await _googleGeocodingService.GetAddressDetailsByPlaceIdAsync(request.PlaceId);
            }
            else if (!string.IsNullOrWhiteSpace(request.Destination))
            {
                result = await _googleGeocodingService.GetAddressDetailsAsync(request.Destination);
            }
            else
            {
                return OperationResult<SearchLocationDto>.Fail("Не передан ни PlaceId, ни текст для поиска локации.");
            }

            if (result == null)
            {
                return OperationResult<SearchLocationDto>.Fail("Не удалось найти локацию по переданным данным.");
            }

            return OperationResult<SearchLocationDto>.Success(new SearchLocationDto
            {
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                FormattedAddress = result.FormattedAddress
            });

        }
    }
}

