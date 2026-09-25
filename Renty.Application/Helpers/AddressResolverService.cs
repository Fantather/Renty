using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Domain.ServiceModels.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    public class AddressResolverService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IGoogleGeocodingService _geocodingService;
        private readonly ILocationResolverService _locationResolver;

        public AddressResolverService(
            IAddressRepository addressRepository,
            IGoogleGeocodingService geocodingService,
            ILocationResolverService locationResolver)
        {
            _addressRepository = addressRepository;
            _geocodingService = geocodingService;
            _locationResolver = locationResolver;
        }

        public async Task<OperationResult<Address>> ResolveAsync(SavePropertyAddressDto dto, CancellationToken ct = default)
        {

            // Если есть placeId
            if (!string.IsNullOrWhiteSpace(dto.PlaceId))
            {
                var existing = await _addressRepository.GetByPlaceIdAsync(dto.PlaceId, ct);
                if (existing != null)
                    return OperationResult<Address>.Success(existing);
                
            }

            if(dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                var existing = await _addressRepository.GetByLocationAsync(dto.Latitude.Value, dto.Longitude.Value);
                if (existing != null)
                    return OperationResult<Address>.Success(existing);
            }

            AddressDetailsDto? geoResult;

            if (!string.IsNullOrWhiteSpace(dto.PlaceId))
            {
                geoResult = await _geocodingService.GetAddressDetailsByPlaceIdAsync(dto.PlaceId);
            }

            // Если есть координаты, revers geocode
            if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                geoResult = await _geocodingService.GetAddressByCoordinatesAsync(dto.Latitude.Value, dto.Longitude.Value);
            }
            // Если только строка адреса
            else if (!string.IsNullOrWhiteSpace(dto.RawAddress))
            {
                geoResult = await _geocodingService.GetAddressDetailsAsync(dto.RawAddress);
            }
            else
            {
                return OperationResult<Address>.Fail("Недостаточно данных для определения адреса");
            }

            if(geoResult == null)
                return OperationResult<Address>.Fail("Не удалось определить координаты адреса. Укажите точку на карте вручную.");

            if (!geoResult.HasStreet || !geoResult.HasStreetNumber)
                return OperationResult<Address>.Fail("Пожалуйста, укажите полный адрес с названием улицы и номером дома, а не только город/район");

            // Если появился placeId после geocode
            if (!string.IsNullOrWhiteSpace(geoResult.PlaceId))
            {
                var existing = await _addressRepository.GetByPlaceIdAsync(geoResult.PlaceId, ct);
                if (existing != null)
                    return OperationResult<Address>.Success(existing);
            }

            var country = await _locationResolver.ResolveCountryAsync(geoResult.CountryName, geoResult.CountryCode, ct);
            var city = await _locationResolver.ResolveCityAsync(geoResult.CityName, country.Id, geoResult.CountryName, geoResult.RegionName, ct);

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            var address = new Address
            {
                PlaceId = geoResult.PlaceId ?? dto.PlaceId,
                FullAddress = geoResult.FormattedAddress,
                Street = geoResult.StreetName,
                District = geoResult.RegionName,
                Location = geometryFactory.CreatePoint(new Coordinate(geoResult.Longitude, geoResult.Latitude)),
                CityId = city.Id,
                City = city
            };

            await _addressRepository.AddAsync(address, ct);

            return OperationResult<Address>.Success(address);
        }
    }
}
