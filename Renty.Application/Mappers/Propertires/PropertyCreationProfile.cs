using AutoMapper;
using NetTopologySuite;
using Renty.Application.DTOs.CreateProperty;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using NetTopologySuite.Geometries;


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
        }
    }
}
