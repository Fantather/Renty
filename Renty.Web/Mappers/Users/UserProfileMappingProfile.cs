using AutoMapper;
using Renty.Application.DTOs.GetReviews;
using Renty.Application.DTOs.GetUser;
using Renty.Web.Models.InputModels.Users;
using Renty.Web.Models.Shared;
using Renty.Web.Models.Users;

namespace Renty.Web.Mappers.Users
{
    public class UserProfileMappingProfile : Profile
    {
        public UserProfileMappingProfile()
        {
            CreateMap<UserFactDto, UserFactViewModel>();
            CreateMap<UserFactInputDto, UserFactInputModel>();
            CreateMap<UserFactInputModel, UserFactInputDto>();

            CreateMap<ReviewDto, ReviewViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : string.Empty))
                .ForMember(dest => dest.AuthorAvatarUrl, opt => opt.MapFrom(src => src.Author != null ? src.Author.AvatarUrl : null))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Content));

            CreateMap<LanguageOptionDto, LanguageOptionViewModel>();

            CreateMap<EditUserProfileInputDto, EditUserProfileInputModel>().ReverseMap();

            CreateMap<GetUserProfileResponse, UserProfileViewModel>()
                .ForMember(dest => dest.Facts, opt => opt.MapFrom(src => src.Facts))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));
        }
    }
}

