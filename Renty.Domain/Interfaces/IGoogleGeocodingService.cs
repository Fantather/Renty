using Renty.Domain.ServiceModels.Locations;

namespace Renty.Domain.Interfaces
{
    public interface IGoogleGeocodingService
    {
        Task<AddressDetailsDto?> GetAddressDetailsAsync(string address, CancellationToken ct = default);
        Task<AddressDetailsDto?> GetAddressByCoordinatesAsync(double latitude, double longitude, CancellationToken ct = default);
        Task<AddressDetailsDto?> GetAddressDetailsByPlaceIdAsync(string placeId, CancellationToken ct = default);
        Task<(double Lat, double Lng)?> GetCityCenterCoordinatesAsync(string cityName, string? countryName, CancellationToken ct = default);
    }
}
