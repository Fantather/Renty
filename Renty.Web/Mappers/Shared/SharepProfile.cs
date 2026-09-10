using AutoMapper;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetProperty;
using Renty.Application.DTOs.GetReviews;
using Renty.Web.Models.Properties;


namespace Renty.Web.Mappers.Shared
{
    public class SharedProfile : Profile
    {
        public SharedProfile()
        {
            CreateMap<HostDto, HostViewModel>()
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ResponseSpeed, opt => opt.MapFrom(src => src.ResponseSpeed))
                .ForMember(dest => dest.IsSuperhost, opt => opt.MapFrom(src => src.IsVerified))
                .ForMember(dest => dest.YearsHosting, opt => opt.MapFrom(src => DateTime.UtcNow.Year - src.CreatedAt.Year));

            CreateMap<AmenitiesDto, AmenityViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IconName, opt => opt.MapFrom(src => src.IconUrl));

            CreateMap<RoomDto, RoomViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.FirstOrDefault().ImageUrl : string.Empty))

                .ForMember(dest => dest.RoomTypeId, opt => opt.MapFrom(src => Guid.Empty));

            CreateMap<ReviewDto, ReviewViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForMember(dest => dest.AuthorAvatarUrl, opt => opt.MapFrom(src => src.Author.AvatarUrl))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}