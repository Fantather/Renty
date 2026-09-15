
using AutoMapper;
using MediatR;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Renty.Application.Commands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Repository;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, OperationResult<Guid>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IGoogleGeocodingService _geocodingService;
        private readonly ILocationResolverService _locationResolver; 
        private readonly ICityRepository _cityRepository; 
        private readonly IMapper _mapper;

        public CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IGoogleGeocodingService geocodingService,
        ILocationResolverService locationResolver,
        ICityRepository cityRepository,
        IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _geocodingService = geocodingService;
            _locationResolver = locationResolver;
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;

            //если не пришло с фронтенда координаты, если пришел город - сперва надо спросить их в бд
            if ((!dto.Latitude.HasValue || !dto.Longitude.HasValue) && !string.IsNullOrWhiteSpace(dto.CityName))
            {
                var existingCity = await _cityRepository.GetCityByNameAsync(dto.CityName, cancellationToken);

                if (existingCity != null && existingCity.Latitude.HasValue && existingCity.Longitude.HasValue)
                {
                    dto.Latitude = (double)existingCity.Latitude.Value;
                    dto.Longitude = (double)existingCity.Longitude.Value;

                    if (string.IsNullOrWhiteSpace(dto.CountryName) && existingCity.Country != null)
                    {
                        dto.CountryName = existingCity.Country.Name;
                        dto.CountryCode = existingCity.Country.CountryCode;
                    }
                }
            }

            //уно реверс, если пришли координаты, но не пришел город
            if (dto.Latitude.HasValue && dto.Longitude.HasValue && string.IsNullOrWhiteSpace(dto.CityName))
            {
                var reverseResult = await _geocodingService.GetAddressByCoordinatesAsync(dto.Latitude.Value, dto.Longitude.Value);
                if (reverseResult != null)
                {
                    _mapper.Map(reverseResult, dto); 
                }
            }

            //если нет и в бд тогда дергать апи гугла
            if (!dto.Latitude.HasValue || !dto.Longitude.HasValue || string.IsNullOrWhiteSpace(dto.CityName))
            {
                var geocodeResult = await _geocodingService.GetAddressDetailsAsync(dto.RawAddress);
                if (geocodeResult == null)
                {
                    return OperationResult<Guid>.Fail("Не удалось определить координаты адреса. Укажите точку на карте вручную.");
                }

                // Заполняем DTO
                _mapper.Map(geocodeResult, dto);
            }


            // найти или создать
            var country = await _locationResolver.ResolveCountryAsync(dto.CountryName, dto.CountryCode, cancellationToken);
            var city = await _locationResolver.ResolveCityAsync(dto.CityName, country.Id, dto.Latitude.Value, dto.Longitude.Value, dto.District, cancellationToken);

            // перенос в проперти
            var property = _mapper.Map<Property>(dto);

            //а это айди городов из бд
            property.CityId = city.Id;
            property.CountryId = country.Id;

            await _propertyRepository.AddAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }


    }
}