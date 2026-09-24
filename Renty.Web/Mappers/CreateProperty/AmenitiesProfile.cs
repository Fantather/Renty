using AutoMapper;
using Renty.Application.DTOs.GetAmenities;
using Renty.Web.Models.Shared;

namespace Renty.Web.Mappers.CreateProperty
{
    public class AmenitiesProfile : Profile
    {
        public AmenitiesProfile()
        {
            CreateMap<AmenityItem,AmenityViewModel>();
        }
    }
}
