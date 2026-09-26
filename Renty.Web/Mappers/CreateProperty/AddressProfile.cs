using AutoMapper;
using Renty.Application.DTOs.CreateProperty;
using Renty.Web.Models.InputModels.Properties;

namespace Renty.Web.Mappers.CreateProperty
{
    public class AddressProfile:Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressInputModel, SavePropertyAddressDto>();
        }
    }
}
