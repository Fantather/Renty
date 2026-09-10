using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Seeders.location.DTO
{
    public class CountrySeedDto
    {
        [JsonPropertyName("iso2")] public string Iso2 { get; set; } = string.Empty;
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("name_ru")] public string NameRu { get; set; } = string.Empty;
        [JsonPropertyName("currency")] public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("phone_code")] public string PhoneCode { get; set; } = string.Empty;
    }
}
