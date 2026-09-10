using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.GoogleGeocoding.Models
{
    public class GoogleGeocodeResult
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; } = new();

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = string.Empty;

        [JsonPropertyName("geometry")]
        public GeometryData Geometry { get; set; } = new();
    }
}
