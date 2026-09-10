using Renty.Domain.ServiceModels.Locations;

namespace Renty.Domain.Interfaces
{
    public interface IGoogleGeocodingService
    {
        Task<AddressDetailsDto?> GetAddressDetailsAsync(string address);
    }
}
