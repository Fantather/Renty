using AutoMapper;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetProperty;
using Renty.Domain.Models;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Models.User;

namespace Renty.Application.Mappers.Properties
{
    public class PropertyDetailsProfile : Profile
    {
        public PropertyDetailsProfile()
        {

            CreateMap<Room, RoomDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<PropertiesCategory, CategoryDto>();
            CreateMap<PropertyImage, ImageDto>();
            CreateMap<Anemities, AmenitiesDto>();


            CreateMap<Property, GetPropertyDetailsResponse>()
                // Переименования полей верхнего уровня
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))

                // Распаковка вложенного объекта Details
                .ForMember(dest => dest.MaxGuests, opt => opt.MapFrom(src => src.Details.MaxGuests))
                .ForMember(dest => dest.BedsCount, opt => opt.MapFrom(src => src.Details.BedsCount))
                .ForMember(dest => dest.BedroomsCount, opt => opt.MapFrom(src => src.Details.BedroomsCount))
                .ForMember(dest => dest.BathroomsCount, opt => opt.MapFrom(src => src.Details.BathroomsCount))
                .ForMember(dest => dest.FloorsCount, opt => opt.MapFrom(src => src.Details.FloorsCount))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Details.Floor))

                // Обход промежуточных таблиц
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.PropertyAmenities.Select(pa => pa.Amenity)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PropertyTags.Select(pt => pt.Tag)))

                // Прямой маппинг коллекций
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))

                //зависит от пользователя, должно быть в Handler
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore());

        }
    }
}