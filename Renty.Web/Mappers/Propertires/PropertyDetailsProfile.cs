using AutoMapper;
using Renty.Application.DTOs.GetProperty;
using Renty.Web.Models.Properties;
using System;
using System.Linq;

namespace Renty.Web.Mappers.Properties
{
    public class PropertyDetailsProfile : Profile
    {
        public PropertyDetailsProfile()
        {

            CreateMap<RatingBreakdownDto, RatingBreakdownViewModel>();


            CreateMap<GetPropertyDetailsResponse, PropertyDetailsViewModel>()

                // Названия отличаются
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PropertyName))
                .ForMember(dest => dest.OverallRating, opt => opt.MapFrom(src => src.AverageRating))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CityName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryName))
                .ForMember(dest => dest.Beds, opt => opt.MapFrom(src => src.BedsCount))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.BedroomsCount))
                .ForMember(dest => dest.Bathrooms, opt => opt.MapFrom(src => src.BathroomsCount))

                // Достать картинки и собрать их в список строк
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                    src.Images != null
                        ? src.Images.Select(img => img.ImageUrl).ToList()
                        : new List<string>()))

                // Собрать кортежи из диапазонов дат бронирования
                .ForMember(dest => dest.BookedRanges, opt => opt.MapFrom(src =>
                    src.BookedRanges != null
                        ? src.BookedRanges.Select(b => new ValueTuple<DateTime, DateTime>(b.From, b.To)).ToList()
                        : new List<(DateTime, DateTime)>()));
        }
    }
}