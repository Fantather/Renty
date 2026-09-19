using AutoMapper;
using Renty.Application.DTOs.GetProperty;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.User;
using System.Linq;

namespace Renty.Application.Mappers.Properties
{
    public class PropertyDetailsProfile : Profile
    {
        public PropertyDetailsProfile()
        {
            CreateMap<ApplicationUser, HostDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src =>
                    src.Languages != null && src.Languages.Any()
                        ? string.Join(", ", src.Languages.Select(l => l.Name))
                        : string.Empty))
                .ForMember(dest => dest.ResponseSpeed, opt => opt.MapFrom(src => src.ResponseSpeed ?? "Неизвестно"));

            CreateMap<Property, GetPropertyDetailsResponse>()
                // Переименования полей
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.FullAddress))

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

                // Маппинг коллекции скидок с фильтрацией 
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src =>
                    src.Discounts != null ? src.Discounts.Where(d => d.IsActive) : null))
                // Маппинг хоста
                .ForMember(dest => dest.Host, opt => opt.MapFrom(src => src.Host))
                // Маппинг отзывы 
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))

                // Поля в Handler
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.BookedRanges, opt => opt.Ignore())
                .ForMember(dest => dest.RatingBreakdown, opt => opt.Ignore())

                // Координаты (если они есть в сущности, укажите MapFrom)
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Address.Location != null ? src.Address.Latitude : (double?)null))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Address.Location != null ? src.Address.Longitude : (double?)null));
        }
    }
}
