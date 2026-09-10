using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.GoogleGeocoding.Models
{
    public class LocationData
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }
}
