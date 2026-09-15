using AutoMapper;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Renty.Application.DTOs.CreateProperty;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Domain.ServiceModels.Locations;


namespace Renty.Application.Mappers.Propertires
{
    public class PropertyCreationProfile : Profile
    {
        public PropertyCreationProfile()
        {
            //Геометрия
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            CreateMap<CreatePropertyDto, Property>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.RawAddress))

                // Cтатус по умолчанию при создании черновика
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PropertyStatusEnum.Draft))

                // конвертер в поинт
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                    src.Longitude.HasValue && src.Latitude.HasValue
                        ? geometryFactory.CreatePoint(new Coordinate(src.Longitude.Value, src.Latitude.Value))
                        : null))


                // игнор
                .ForMember(dest => dest.CityId, opt => opt.Ignore())
                .ForMember(dest => dest.CountryId, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore());


            //для обратного геокодирования
            CreateMap<AddressDetailsDto, CreatePropertyDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.CityName, opt =>
                {
                    opt.Condition((src, dest) => string.IsNullOrWhiteSpace(dest.CityName));
                    opt.MapFrom(src => src.CityName);
                })
                .ForMember(dest => dest.Street, opt =>
                {
                    opt.Condition((src, dest) => string.IsNullOrWhiteSpace(dest.Street));
                    opt.MapFrom(src => src.StreetName);
                })
                .ForMember(dest => dest.CountryName, opt =>
                {
                    opt.Condition((src, dest) => string.IsNullOrWhiteSpace(dest.CountryName));
                    opt.MapFrom(src => src.CountryName);
                })
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition((src, dest) => string.IsNullOrWhiteSpace(dest.CountryCode));
                    opt.MapFrom(src => src.CountryCode);
                })
                .ForMember(dest => dest.District, opt =>
                {
                    opt.Condition((src, dest) => string.IsNullOrWhiteSpace(dest.District));
                    opt.MapFrom(src => src.RegionName); 
                });
        }


    }
}
