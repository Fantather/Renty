using AutoMapper;
using Renty.Application.DTOs.GetProperties;
using Renty.Domain.Models.Properties;

namespace Renty.Application.Mappers.Propertires
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyListItem>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src =>
                    src.PropertyImages.FirstOrDefault(i => i.IsPrimary).ImageUrl
                    ?? src.PropertyImages.FirstOrDefault().ImageUrl));
            

            // CreateMap<Property, PropertyDetailsDto>();
            // CreateMap<Room, RoomDto>();
        }
    }
}