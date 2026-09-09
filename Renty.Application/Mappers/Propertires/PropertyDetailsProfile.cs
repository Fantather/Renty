using AutoMapper;
using Renty.Application.DTOs.GetProperty;
using Renty.Domain.Models.Properties;
using System.Linq;

namespace Renty.Application.Mappers.Properties
{
    public class PropertyDetailsProfile : Profile
    {
        public PropertyDetailsProfile()
        {
            CreateMap<Property, GetPropertyDetailsResponse>()
                // Переименования полей
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))

                // Распаковка Details
                .ForMember(dest => dest.MaxGuests, opt => opt.MapFrom(src => src.Details.MaxGuests))
                .ForMember(dest => dest.BedsCount, opt => opt.MapFrom(src => src.Details.BedsCount))
                .ForMember(dest => dest.BedroomsCount, opt => opt.MapFrom(src => src.Details.BedroomsCount))
                .ForMember(dest => dest.BathroomsCount, opt => opt.MapFrom(src => src.Details.BathroomsCount))
                .ForMember(dest => dest.FloorsCount, opt => opt.MapFrom(src => src.Details.FloorsCount))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Details.Floor))

                .ForMember(dest => dest.RoomsCount, opt => opt.MapFrom(src => src.Details.RoomsCount))

                // Обход промежуточных таблиц
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.PropertyAmenities.Select(pa => pa.Amenity)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PropertyTags.Select(pt => pt.Tag)))

                // коллекции
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))

                // Маппинг отзывы 
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))

                // Поля в Handler
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.BookedRanges, opt => opt.Ignore())
                .ForMember(dest => dest.RatingBreakdown, opt => opt.Ignore())

                // Координаты (если они есть в сущности, укажите MapFrom)
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location != null ? src.Location.Y : (double?)null))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location != null ? src.Location.X : (double?)null));
        }
    }
}