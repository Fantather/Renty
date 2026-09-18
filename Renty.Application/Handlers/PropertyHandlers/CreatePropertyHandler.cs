
using AutoMapper;
using MediatR;
using Microsoft.VisualBasic;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Renty.Application.Commands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Helpers;
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
        private readonly IAddressRepository _addressRepository;
        private readonly AddressResolverService _addressResolverService;
        private readonly IMapper _mapper;

        public CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IGoogleGeocodingService geocodingService,
        ILocationResolverService locationResolver,
        ICityRepository cityRepository,
        IAddressRepository addressRepository,
        AddressResolverService addressResolverService,
        IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _geocodingService = geocodingService;
            _locationResolver = locationResolver;
            _cityRepository = cityRepository;
            _addressRepository = addressRepository;
            _addressResolverService = addressResolverService;
            _mapper = mapper;
        }

        public async Task<OperationResult<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;

            ////если не пришло с фронтенда координаты, если пришел адрес - проверка в базе
            //if ((!dto.Latitude.HasValue || !dto.Longitude.HasValue) && !string.IsNullOrWhiteSpace(dto.PlaceId))
            //{
            //    var existingAddress = await _addressRepository.GetByPlaceIdAsync(dto.PlaceId, cancellationToken);

            //    if (existingAddress != null && existingAddress.Location != null)
            //    {
            //        dto.Latitude = (double)existingAddress.Latitude!.Value;
            //        dto.Longitude = (double)existingAddress.Longitude!.Value;

            //        if(existingAddress.City != null)
            //        {
            //            if (string.IsNullOrWhiteSpace(dto.CityName))
            //            {
            //                dto.CityName = existingAddress.City.NameRu ?? existingAddress.City.Name;
            //            }
            //            if (string.IsNullOrWhiteSpace(dto.CountryName) && existingAddress.City.Country != null)
            //            {
            //                dto.CountryName = existingAddress.City.Country.Name;
            //                dto.CountryCode = existingAddress.City.Country.CountryCode;
            //            }
            //        }

            //    }
            //}

            ////уно реверс, если пришли координаты, но не пришел адрес
            //if (dto.Latitude.HasValue && dto.Longitude.HasValue && string.IsNullOrWhiteSpace(dto.PlaceId))
            //{
            //    var reverseResult = await _geocodingService.GetAddressByCoordinatesAsync(dto.Latitude.Value, dto.Longitude.Value);
            //    if (reverseResult != null)
            //    {
            //        _mapper.Map(reverseResult, dto); 
            //    }
            //}

            ////если нет и в бд тогда дергать апи гугла
            //else if (!string.IsNullOrWhiteSpace(dto.RawAddress))
            //{
            //    var geocodeResult = await _geocodingService.GetAddressDetailsAsync(dto.RawAddress);
            //    if (geocodeResult == null)
            //    {
            //        return OperationResult<Guid>.Fail("Не удалось определить координаты адреса. Укажите точку на карте вручную.");
            //    }

            //    // Заполняем DTO
            //    _mapper.Map(geocodeResult, dto);
            //}

            //// если после обратного геокодирования появился placeId 
            //// то проверяем нет ли его в базе данных
            //if (!string.IsNullOrWhiteSpace(dto.PlaceId))
            //{
            //    var existingAddress = await _addressRepository.GetByPlaceIdAsync(dto.PlaceId, cancellationToken);

            //    if (existingAddress != null && existingAddress.Location != null)
            //    {
            //        dto.Latitude = (double)existingAddress.Latitude!.Value;
            //        dto.Longitude = (double)existingAddress.Longitude!.Value;

            //        if (existingAddress.City != null)
            //        {
            //            if (string.IsNullOrWhiteSpace(dto.CityName))
            //            {
            //                dto.CityName = existingAddress.City.NameRu ?? existingAddress.City.Name;
            //            }
            //            if (string.IsNullOrWhiteSpace(dto.CountryName) && existingAddress.City.Country != null)
            //            {
            //                dto.CountryName = existingAddress.City.Country.Name;
            //                dto.CountryCode = existingAddress.City.Country.CountryCode;
            //            }
            //        }
            //    }
            //}

            //// найти или создать
            //var country = await _locationResolver.ResolveCountryAsync(dto.CountryName, dto.CountryCode, cancellationToken);
            //var city = await _locationResolver.ResolveCityAsync(dto.CityName, country.Id, dto.Latitude.Value, dto.Longitude.Value, dto.District, cancellationToken);

            var addressResult = await _addressResolverService.ResolveAsync(dto, cancellationToken);
            if(!addressResult.IsSuccess)
                return OperationResult<Guid>.Fail(addressResult.Errors.ToArray());

            // перенос в проперти
            var property = _mapper.Map<Property>(dto);

            //а это айди городов из бд
            property.CityId = addressResult.Data!.CityId!;
            property.CountryId = addressResult.Data.City.CountryId;
            property.AddressId = addressResult.Data!.Id;

            await _propertyRepository.AddAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
