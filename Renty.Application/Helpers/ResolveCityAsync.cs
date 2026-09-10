using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;


namespace Renty.Application.Services
{
    public class LocationResolverService : ILocationResolverService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICityRepository _cityRepository;

        public LocationResolverService(
            ICountryRepository countryRepository,
            IRegionRepository regionRepository,
            ICityRepository cityRepository)
        {
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _cityRepository = cityRepository;
        }

        public async Task<Country> ResolveCountryAsync(string? countryName, string? countryCode, CancellationToken ct = default)
        {
            var name = string.IsNullOrWhiteSpace(countryName) ? "Unknown" : countryName;
            var code = string.IsNullOrWhiteSpace(countryCode) ? "XX" : countryCode;

            var matches = await _countryRepository.GetCountriesByNameAsync(name, 1, ct);
            var country = matches.FirstOrDefault();

            if (country == null)
            {
                country = new Country
                {
                    Name = name,
                    CountryCode = code
                };
                await _countryRepository.AddAsync(country, ct);
            }

            return country;
        }

        public async Task<City> ResolveCityAsync(string? cityName, Guid countryId, double lat, double lng, string? regionName, CancellationToken ct = default)
        {
            var name = string.IsNullOrWhiteSpace(cityName) ? "Unknown" : cityName;
            var cities = await _cityRepository.GetCitiesByCountryAsync(countryId, ct);
            var city = cities.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                                                 (!string.IsNullOrEmpty(c.NameRu) && c.NameRu.Equals(name, StringComparison.OrdinalIgnoreCase)));

            if (city == null)
            {
                Guid? regionId = null;
                if (!string.IsNullOrWhiteSpace(regionName))
                {
                    var regions = await _regionRepository.GetRegionsByCountryIdAsync(countryId, ct);
                    var region = regions.FirstOrDefault(r => r.Name.Equals(regionName, StringComparison.OrdinalIgnoreCase));
                    if (region == null)
                    {
                        region = new Region { Name = regionName, CountryId = countryId };
                        await _regionRepository.AddAsync(region, ct);
                    }
                    regionId = region.Id;
                }

                city = new City
                {
                    Name = name,
                    CountryId = countryId,
                    RegionId = regionId,
                    Latitude = (decimal)lat,
                    Longitude = (decimal)lng
                };
                await _cityRepository.AddAsync(city, ct);
            }

            return city;
        }
    }
}