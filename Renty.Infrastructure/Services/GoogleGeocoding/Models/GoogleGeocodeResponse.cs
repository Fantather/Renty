using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.GoogleGeocoding.Models
{
    public class GoogleGeocodeResponse
    {
        [JsonPropertyName("results")]
        public List<GoogleGeocodeResult> Results { get; set; } = new();

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
