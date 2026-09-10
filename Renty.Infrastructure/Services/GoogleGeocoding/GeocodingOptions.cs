using System.Text.Json.Serialization;
using Renty.Infrastructure.Services.GoogleGeocoding.Models;

namespace Renty.Infrastructure.Services.GoogleGeocoding
{

    public class GeocodingOptions
    {
        public const string SectionName = "GeocodingSettings";
        public string GeocodingApiKey { get; set; } = string.Empty;
    }


}

