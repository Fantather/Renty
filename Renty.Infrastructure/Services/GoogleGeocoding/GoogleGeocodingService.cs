using Microsoft.Extensions.Options;
using Renty.Domain.Interfaces;
using Renty.Domain.ServiceModels.Locations;
using Renty.Infrastructure.Services.GoogleGeocoding.Models;
using System.Globalization;
using System.Text.Json;

namespace Renty.Infrastructure.Services.GoogleGeocoding
{
    public class GoogleGeocodingService : IGoogleGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly GeocodingOptions _options;

        private readonly string _baseUri = "https://maps.googleapis.com/maps/api/geocode/json";

        public GoogleGeocodingService(HttpClient httpClient, IOptions<GeocodingOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<AddressDetailsDto?> GetAddressByCoordinatesAsync(double latitude, double longitude)
        {
            var lat = latitude.ToString(CultureInfo.InvariantCulture);
            var lng = longitude.ToString(CultureInfo.InvariantCulture);

            var requestUri = $"{_baseUri}?latlng={lat},{lng}&key={_options.GeocodingApiKey}&language=ru";

            var response = await _httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var geocodeResponse = JsonSerializer.Deserialize<GoogleGeocodeResponse>(json);

            if (geocodeResponse == null || geocodeResponse.Status != "OK" || !geocodeResponse.Results.Any())
            {
                return null;
            }

            var result = geocodeResponse.Results.First();
            var dto = new AddressDetailsDto
            {
                FormattedAddress = result.FormattedAddress,
                Latitude = latitude,
                Longitude = longitude
            };

            ParseAddressComponents(result.AddressComponents, dto);

            return dto;
        }

        public async Task<AddressDetailsDto?> GetAddressDetailsAsync(string address)
        {
            var requestUri = $"{_baseUri}?address={Uri.EscapeDataString(address)}&key={_options.GeocodingApiKey}&language=ru";

            var response = await _httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var geocodeResponse = JsonSerializer.Deserialize<GoogleGeocodeResponse>(json);

            if (geocodeResponse == null || geocodeResponse.Status != "OK" || !geocodeResponse.Results.Any())
            {
                return null;
            }

            var result = geocodeResponse.Results.First();
            var dto = new AddressDetailsDto
            {
                FormattedAddress = result.FormattedAddress,
                Latitude = result.Geometry.Location.Lat,
                Longitude = result.Geometry.Location.Lng
            };

            ParseAddressComponents(result.AddressComponents, dto);

            return dto;
        }

        // Общий приватный метод
        private void ParseAddressComponents(List<AddressComponent> components, AddressDetailsDto dto)
        {
            // Разбор компонентов адреса
            foreach (var component in components)
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
                else if (component.Types.Contains("route"))
                {
                    dto.StreetName = component.LongName;
                }
            }

            // если гугл вернет другой тип для города
            if (string.IsNullOrWhiteSpace(dto.CityName))
            {
                var fallbackCity = components.FirstOrDefault(c => c.Types.Contains("administrative_area_level_2"));
                if (fallbackCity != null)
                {
                    dto.CityName = fallbackCity.LongName;
                }
            }
        }
    }
}