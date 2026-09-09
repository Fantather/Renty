using AutoMapper;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Domain.Models;

namespace Renty.Application.Mappers.Common
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<City, CityDto>();
            CreateMap<PropertiesCategory, CategoryDto>();

            // Если ImageDto содержит просто URL
            CreateMap<PropertyImage, ImageDto>();

            // Для удобств
            CreateMap<Anemities, AmenitiesDto>();
        }
    }
}
