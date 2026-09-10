using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Seeders.location.DTO
{
    public class CitySeedDto
    {
        [JsonPropertyName("country_iso2")] public string CountryIso2 { get; set; } = string.Empty;
        [JsonPropertyName("region_code")] public string RegionCode { get; set; } = string.Empty;
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("name_ru")] public string NameRu { get; set; } = string.Empty;
        [JsonPropertyName("latitude")] public decimal Latitude { get; set; }
        [JsonPropertyName("longitude")] public decimal Longitude { get; set; }
    }
}
