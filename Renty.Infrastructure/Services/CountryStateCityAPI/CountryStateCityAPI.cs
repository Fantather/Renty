using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Renty.Infrastructure.Services.CountryStateCityAPI
{
    public class CountryStateCityAPI
    {
        private readonly CountryStateCityApiOptions _options;

        public CountryStateCityAPI(IOptions<CountryStateCityApiOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<Country>> GetAllCountryAsync()
        {
            try
            {
                //E:\Programming\Курсовой\Project 4\Renty\Renty.Infrastructure\Services\JsonData
                var path = Path.Combine("..", "Renty.Infrastructure", "Services", "JsonData", "countries.json");
                //var json = await $"{_options.BASE_URL}/countries"
                //    .WithHeader("X-CSCAPI-KEY", _options.X_CSCAPI_KEY)
                //    .GetStringAsync();
                var json = await File.ReadAllTextAsync(path);

                return JsonSerializer.Deserialize<List<Country>>(json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error message: "+ex.Message);
            }

            return new List<Country>();
        }

        public async Task<List<City>> GetCitiesByCountriISO2(string iso2)
        {
            try
            {
                var path = Path.Combine("..", "Renty.Infrastructure", "Services", "JsonData", "cities.json");
                //var json = await $"{_options.BASE_URL}/countries/{iso2}/cities"
                //    .WithHeader("X-CSCAPI-KEY", _options.X_CSCAPI_KEY)
                //    .GetStringAsync();
                var json = await File.ReadAllTextAsync(path);

                return JsonSerializer.Deserialize<List<City>>(json);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: " + ex.Message);
            }

            return new List<City> { };
        }
        public async Task<List<City>> GetCitiesBySearchTerm(string stringTerm)
        {
            try
            {
                //if (string.IsNullOrEmpty(stringTerm) || stringTerm.Length < 2 || stringTerm.Length < 100)
                //    return new List<City>();


                var path = Path.Combine("..", "Renty.Infrastructure", "Services", "JsonData", "cities.json");

                var json = await File.ReadAllTextAsync(path);

                var cities = JsonSerializer.Deserialize<List<City>>(json);

                return cities.Where(c => c.Name.ToLower().StartsWith(stringTerm.ToLower())).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: " + ex.Message);
            }

            return new List<City> { };
        }

        public async Task<List<City>> GetAllCity()
        {
            try
            {
                var path = Path.Combine("..", "Renty.Infrastructure", "Services", "JsonData", "cities.json");

                var json = await File.ReadAllTextAsync(path);

                var cities = JsonSerializer.Deserialize<List<City>>(json);

                return cities;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: " + ex.Message);
            }

            return new List<City> { };
        }

        public async Task<List<State>> GetStatesByCountryISO2(string country_iso2)
        {
            try
            {
                var path = Path.Combine("..", "Renty.Infrastructure", "Services", "JsonData", "states.json");

                var jsonStates = await File.ReadAllTextAsync(path);


                var states = JsonSerializer.Deserialize<List<State>>(jsonStates);

                return states.Where(s=>s.CountryIso2 == country_iso2).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: " + ex.Message);
            }

            return new List<State> { };
        }
    }


    public class State
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("native")]
        public string Native { get; set; } = string.Empty;
        [JsonPropertyName("iso")]
        public string Iso { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;
        [JsonPropertyName("country_id")]
        public string CountryId { get; set; } = string.Empty;
        [JsonPropertyName("country_iso2")]
        public string CountryIso2 { get; set; } = string.Empty;
    }

    public class City
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;
        [JsonPropertyName("country_id")]
        public string CountryId { get; set; } = string.Empty;
        [JsonPropertyName("country_iso2")]
        public string CountryIso2 { get; set; } = string.Empty;
        [JsonPropertyName("state_id")]
        public string StateId { get; set; } = string.Empty;
    }

    public class Country
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("name_ru")]
        public string NameRu { get; set; } = string.Empty;
        [JsonPropertyName("name_uk")]
        public string NameUk { get; set; } = string.Empty;
        [JsonPropertyName("iso2")]
        public string Iso2 { get; set; } = string.Empty;
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;
    }
}
