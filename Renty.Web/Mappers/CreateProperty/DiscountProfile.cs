using AutoMapper;
using Renty.Application.DTOs.CreateProperty;
using Renty.Web.Models.InputModels.Properties;

namespace Renty.Web.Mappers.CreateProperty
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountsInputModel, DiscountsInputDto>();
        }
    }
}
