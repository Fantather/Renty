using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.GoogleGeocoding.Models
{
    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; } = string.Empty;

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = string.Empty;

        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = new();
    }
}
