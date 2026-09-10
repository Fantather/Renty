using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.GoogleGeocoding.Models
{
    public class GeometryData
    {
        [JsonPropertyName("location")]
        public LocationData Location { get; set; } = new();
    }
}
