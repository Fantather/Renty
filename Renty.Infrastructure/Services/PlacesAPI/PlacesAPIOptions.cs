using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Services.PlacesAPI
{
    public class PlacesAPIOptions
    {
        public const string SectionName = "GeocodingSettings";
        public string GeocodingApiKey { get; set; } = string.Empty;
    }
}
