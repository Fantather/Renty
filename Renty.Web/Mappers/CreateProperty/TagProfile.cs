using AutoMapper;
using Renty.Application.DTOs.GetTags;
using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.Shared;

namespace Renty.Web.Mappers.CreateProperty
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagResponse, TagViewModel>();
        }
    }
}
