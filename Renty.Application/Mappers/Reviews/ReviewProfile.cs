
using AutoMapper;
using Renty.Application.DTOs.GetReviews;
using Renty.Domain.Models.User;


namespace Renty.Application.Mappers.Reviews
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ApplicationUser, AuthorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User));

            // CreateMap<HostResponse, HostResponseDto>()
            //    .ForMember(dest => dest.Host, opt => opt.MapFrom(src => src.User));
        }
    }
}
