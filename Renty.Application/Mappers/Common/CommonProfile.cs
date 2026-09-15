using AutoMapper;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Application.DTOs.GetProperty;
using Renty.Application.DTOs.GetReviews;
using Renty.Domain.Models;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Models.User;
using System.Linq;

namespace Renty.Application.Mappers.Common
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<PropertiesCategory, CategoryDto>();
            CreateMap<PropertyImage, ImageDto>();
            CreateMap<Anemities, AmenitiesDto>();
            CreateMap<Discount, DiscountDto>();

            CreateMap<ApplicationUser, AuthorDto>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            // Добавленный маппинг для HostDto
            CreateMap<ApplicationUser, HostDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src =>
                    src.Languages != null && src.Languages.Any()
                        ? string.Join(", ", src.Languages.Select(l => l.Name))
                        : string.Empty))
                .ForMember(dest => dest.ResponseSpeed, opt => opt.MapFrom(src => src.ResponseSpeed ?? "Неизвестно"));
        }

    }
}
