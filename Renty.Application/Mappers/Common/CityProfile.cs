using AutoMapper;
using Renty.Domain.Models.Locations;
using Renty.Application.DTOs.GetCities;

namespace Renty.Application.Mappers.Common
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>()

                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Name))


                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Name : null))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name));
        }
    }
}