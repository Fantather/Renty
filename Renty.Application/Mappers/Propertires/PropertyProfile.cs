using AutoMapper;
using Renty.Application.DTOs.GetProperties;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.User;
using System.Linq;

namespace Renty.Application.Mappers.Properties
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            


            CreateMap<Property, PropertyListItem>()
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))

                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src =>
                    src.PropertyImages.Where(i => i.IsPrimary).Select(i => i.ImageUrl).FirstOrDefault() ??
                    src.PropertyImages.Select(i => i.ImageUrl).FirstOrDefault() ??
                    string.Empty)) 

                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.Duration, opt => opt.Ignore());
        }
    }
}