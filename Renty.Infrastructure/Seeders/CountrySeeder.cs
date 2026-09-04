using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.Locations;
using Renty.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Renty.Infrastructure.Seeders
{
    public static class CountrySeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Countries.AnyAsync())
            {
                return;
            }
            try
            {
                var ukraine = await SeedCountryAsync(context, "Украина", "UA", "UAH", "+380");

                var kyivRegion = await SeedRegionAsync(context, "Киевская", ukraine.Id);
                var odesaRegion = await SeedRegionAsync(context, "Одесская", ukraine.Id);

                await SeedCityAsync(context, "Киев", kyivRegion.Id, ukraine.Id, 50.4501m, 30.5234m);
                await SeedCityAsync(context, "Одесса", odesaRegion.Id, ukraine.Id, 46.4825m, 30.7233m);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации локаций: {ex.Message}");
            }
        }

        private static async Task<Country> SeedCountryAsync(AppDbContext context, string countryName, string countryCode, string currencyCode, string phoneCode)
        {
            if (await context.Countries.AnyAsync(c => c.Name == countryName))
            {
                throw new ArgumentException("Страна уже существует", nameof(countryName));
            }

            var country = new Country
            {
                Id = Guid.CreateVersion7(),
                Name = countryName,
                CountryCode = countryCode,
                CurrencyCode = currencyCode,
                PhoneCode = phoneCode,
                IsActive = true
            };

            await context.Countries.AddAsync(country);
            await context.SaveChangesAsync();

            return country; 
        }

        private static async Task<Region> SeedRegionAsync(AppDbContext context, string regionName, Guid countryId)
        {
            if (!await context.Countries.AnyAsync(c => c.Id == countryId))
            {
                throw new ArgumentException("Страна не существует", nameof(countryId));
            }
            if (await context.Regions.AnyAsync(r => r.Name == regionName && r.CountryId == countryId))
            {
                throw new ArgumentException("Регион уже существует в указанной стране", nameof(regionName));
            }

            var region = new Region
            {
                Id = Guid.CreateVersion7(),
                Name = regionName,
                CountryId = countryId,
                IsActive = true
            };

            await context.Regions.AddAsync(region);
            await context.SaveChangesAsync();

            return region;
        }

        private static async Task<City> SeedCityAsync(AppDbContext context, string cityName, Guid regionId, Guid countryId, decimal latitude, decimal longitude)
        {
            if (!await context.Countries.AnyAsync(c => c.Id == countryId))
            {
                throw new ArgumentException("Страна не существует", nameof(countryId));
            }
            if (!await context.Regions.AnyAsync(r => r.Id == regionId && r.CountryId == countryId))
            {
                throw new ArgumentException("Регион не существует или не принадлежит указанной стране", nameof(regionId));
            }
            if (await context.Cities.AnyAsync(c => c.Name == cityName && c.RegionId == regionId && c.CountryId == countryId))
            {
                throw new ArgumentException("Город уже существует в указанном регионе и стране", nameof(cityName));
            }

            var city = new City
            {
                Id = Guid.CreateVersion7(),
                Name = cityName,
                RegionId = regionId,
                CountryId = countryId,
                Latitude = latitude,
                Longitude = longitude,
                IsActive = true
            };

            await context.Cities.AddAsync(city);
            await context.SaveChangesAsync();

            return city;
        }
    }
}