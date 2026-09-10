using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.Locations;
using Renty.Infrastructure.Seeders.location.DTO;
using System.Text.Json;
using Renty.Infrastructure.Data;

namespace Renty.Infrastructure.Seeders.location;

public class LocationSeeder
{
    public static async Task SeedLocationsAsync(AppDbContext context, string basePath)
    {
        if (await context.Set<Country>().AnyAsync())
        {
            return;
        }

        var countriesJson = await File.ReadAllTextAsync(Path.Combine(basePath, "countries.json"));
        var countriesSeed = JsonSerializer.Deserialize<List<CountrySeedDto>>(countriesJson);
        var countryEntities = new Dictionary<string, Country>();

        if (countriesSeed != null)
        {
            foreach (var c in countriesSeed)
            {
                var country = new Country
                {
                    Name = c.Name,
                    NameRu = c.NameRu,
                    CountryCode = c.Iso2,
                    CurrencyCode = c.Currency,
                    PhoneCode = c.PhoneCode
                };
                countryEntities.Add(c.Iso2, country);
                context.Add(country);
            }
            await context.SaveChangesAsync();
        }

        var regionsJson = await File.ReadAllTextAsync(Path.Combine(basePath, "regions.json"));
        var regionsSeed = JsonSerializer.Deserialize<List<RegionSeedDto>>(regionsJson);
        var regionEntities = new Dictionary<string, Region>();

        if (regionsSeed != null)
        {
            foreach (var r in regionsSeed)
            {
                if (countryEntities.TryGetValue(r.CountryIso2, out var country))
                {
                    var region = new Region
                    {
                        Name = r.Name,
                        NameRu = r.NameRu,
                        CountryId = country.Id
                    };
                    regionEntities.Add($"{r.CountryIso2}_{r.RegionCode}", region);
                    context.Add(region);
                }
            }
            await context.SaveChangesAsync();
        }

        var citiesJson = await File.ReadAllTextAsync(Path.Combine(basePath, "cities.json"));
        var citiesSeed = JsonSerializer.Deserialize<List<CitySeedDto>>(citiesJson);

        if (citiesSeed != null)
        {
            foreach (var cityDto in citiesSeed)
            {
                if (countryEntities.TryGetValue(cityDto.CountryIso2, out var country))
                {
                    Guid? regionId = null;
                    if (!string.IsNullOrEmpty(cityDto.RegionCode) &&
                        regionEntities.TryGetValue($"{cityDto.CountryIso2}_{cityDto.RegionCode}", out var region))
                    {
                        regionId = region.Id;
                    }

                    var city = new City
                    {
                        Name = cityDto.Name,
                        NameRu = cityDto.NameRu,
                        CountryId = country.Id,
                        RegionId = regionId,
                        Latitude = cityDto.Latitude,
                        Longitude = cityDto.Longitude
                    };
                    context.Add(city);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}