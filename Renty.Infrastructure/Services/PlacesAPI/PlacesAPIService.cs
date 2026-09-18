using Microsoft.Extensions.Options;
using Renty.Domain.Interfaces;
using Renty.Domain.ServiceModels.Places;
using Renty.Infrastructure.Services.PlacesAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Renty.Infrastructure.Services.PlacesAPI
{
    public class PlacesAPIService : IAddressAutocompleteService
    {
        private readonly PlacesAPIOptions _options;
        private readonly HttpClient _httpClient;
        public PlacesAPIService(IOptions<PlacesAPIOptions> options, HttpClient httpClient)
        {
            _options = options.Value;
            _httpClient = httpClient;
        }

        public async Task<List<AddressSuggestionDto>> SearchLocations(string input, string sessionToken, string languageCode = "ru", CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(input))
                return new();

            var types = new HashSet<string> { "premise", "geocode" };

            var body = new
            {
                input,
                languageCode,
                sessionToken
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://places.googleapis.com/v1/places:autocomplete") 
            {
                Content = JsonContent.Create(body),
            };

            request.Headers.Add("X-Goog-Api-Key", _options.GeocodingApiKey);
            request.Headers.Add("Accept-Language", languageCode);
            request.Headers.Add("X-Goog-FieldMask", "suggestions.placePrediction.placeId,suggestions.placePrediction.text.text,suggestions.placePrediction.types");

            var response = await _httpClient.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Google Places autocomplete failed: {response.StatusCode}");
                return new();
            }

            var payload = await response.Content.ReadFromJsonAsync<GooglePlacesResponse>(ct);

            if (payload == null)
                return new();

            return payload.Suggestions
                .Where(s => s.PlacePrediction.Types.Any(t => types.Contains(t)))
                .Select(s => new AddressSuggestionDto
                {
                    PlaceId = s.PlacePrediction.PlaceId,
                    Text = s.PlacePrediction.Text.Text
                }).ToList();
        }
    }
}
