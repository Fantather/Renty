using Microsoft.Extensions.Options;
using Renty.Domain.Interfaces;
using Renty.Domain.ServiceModels.Locations;
using Renty.Infrastructure.Services.GoogleGeocoding.Models;
using System.Text.Json;

namespace Renty.Infrastructure.Services.GoogleGeocoding
{
    public class GoogleGeocodingService : IGoogleGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly GeocodingOptions _options;

        public GoogleGeocodingService(HttpClient httpClient, IOptions<GeocodingOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<AddressDetailsDto?> GetAddressDetailsAsync(string address)
        {
            var requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_options.GeocodingApiKey}";

            var response = await _httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var geocodeResponse = JsonSerializer.Deserialize<GoogleGeocodeResponse>(json);

            if (geocodeResponse == null || geocodeResponse.Status != "OK" || !geocodeResponse.Results.Any())
                return null;

            var result = geocodeResponse.Results.First();
            var dto = new AddressDetailsDto
            {
                FormattedAddress = result.FormattedAddress,
                Latitude = result.Geometry.Location.Lat,
                Longitude = result.Geometry.Location.Lng
            };

            // Разбор компонентов адреса
            foreach (var component in result.AddressComponents)
            {
                if (component.Types.Contains("country"))
                {
                    dto.CountryName = component.LongName;
                    dto.CountryCode = component.ShortName;
                }
                else if (component.Types.Contains("administrative_area_level_1"))
                {
                    dto.RegionName = component.LongName;
                }
                else if (component.Types.Contains("locality") || component.Types.Contains("postal_town"))
                {
                    dto.CityName = component.LongName;
                }
            }

            return dto;
        }
    }
}
