using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.PlacesAPI.Models
{
    public class GooglePlacesResponse
    {
        [JsonPropertyName("suggestions")]
        public List<Suggestion> Suggestions { get; set; } = new();
    }

    public class Suggestion
    {
        [JsonPropertyName("placePrediction")]
        public PlacePrediction PlacePrediction { get; set; } = null!;
    }

    public class PlacePrediction
    {
        [JsonPropertyName("placeId")]
        public string PlaceId { get; set; } = null!;
        [JsonPropertyName("text")]
        public TextValue Text { get; set; } = null!;
        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = new();
    }
    public class TextValue
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;
    }
}
