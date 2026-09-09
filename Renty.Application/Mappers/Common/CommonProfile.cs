using AutoMapper;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Application.DTOs.GetProperty;
using Renty.Domain.Models;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.Media;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;

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
        }
    }
}
