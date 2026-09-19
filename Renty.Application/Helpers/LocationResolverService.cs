using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;


namespace Renty.Application.Services
{
    public class LocationResolverService : ILocationResolverService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IGoogleGeocodingService _geocodingService;

        public LocationResolverService(
            ICountryRepository countryRepository,
            IRegionRepository regionRepository,
            ICityRepository cityRepository,
            IGoogleGeocodingService geocodingService)
        {
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _cityRepository = cityRepository;
            _geocodingService = geocodingService;
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

        public async Task<City> ResolveCityAsync(string? cityName, Guid countryId, string? countryName, string? regionName, CancellationToken ct = default)
        {
            var name = string.IsNullOrWhiteSpace(cityName) ? "Unknown" : cityName;
            var city = await _cityRepository.GetCityByNameAndCountryAsync(name, countryId, ct);

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

                // Запрос координат центра города
                var cityCoords = await _geocodingService.GetCityCenterCoordinatesAsync(name, countryName, ct);

                city = new City
                {
                    Name = name,
                    CountryId = countryId,
                    RegionId = regionId,
                    Latitude = cityCoords.HasValue ? (decimal)cityCoords.Value.Lat : null,
                    Longitude = cityCoords.HasValue ? (decimal)cityCoords.Value.Lng : null
                };
                await _cityRepository.AddAsync(city, ct);
            }

            return city;
        }


    }
}
