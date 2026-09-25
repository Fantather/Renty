using Renty.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface ILocationResolverService
    {
        Task<Country> ResolveCountryAsync(string? countryName, string? countryCode, CancellationToken ct = default);
        Task<City> ResolveCityAsync(string? cityName, Guid countryId, string? countryName, string? regionName, CancellationToken ct = default);
        Task<City> ResolveCityFromInputAsync(string? placeId, string? cityText, CancellationToken ct = default);
    }
}
