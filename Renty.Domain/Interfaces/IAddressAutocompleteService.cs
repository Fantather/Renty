using Renty.Domain.ServiceModels.Places;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IAddressAutocompleteService
    {
        Task<List<AddressSuggestionDto>> SearchLocations(string input, string sessionToken, string languageCode = "ru", CancellationToken ct = default);
        Task<List<CitySuggestionDto>> SearchCities(string input, string sessionToken, string languageCode = "ru", CancellationToken ct = default);
    }
}
