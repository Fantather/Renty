using AutoMapper;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.DTOs.Common;
using Renty.Web.Models.Shared;



namespace Renty.Web.Mappers.Home
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            CreateMap<CategoryDto, CategoryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IconName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImageUrl) ? "star" : src.ImageUrl));

            CreateMap<PropertyListItem, PropertyCardViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => new List<string> { src.CoverImage }))
                .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => src.IsFavorite))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CityName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.AverageRating))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.DurationLabel, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.PricePerNight));
        }
    }
}