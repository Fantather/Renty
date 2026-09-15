using Renty.Domain.ServiceModels.Locations;

namespace Renty.Domain.Interfaces
{
    public interface IGoogleGeocodingService
    {
        Task<AddressDetailsDto?> GetAddressDetailsAsync(string address);
        Task<AddressDetailsDto?> GetAddressByCoordinatesAsync(double latitude, double longitude);
    }
}
