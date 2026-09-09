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
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Comment))
                // Маппинг автора отзыва
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))

                // Конструируем объект ответа хоста
                .ForMember(dest => dest.HostResponse, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.HostResponse)
                        ? null // Если ответа нет, возвращаем null
                        : new HostResponseDto
                        {
                            Content = src.HostResponse,
                            CreatedAt = src.HostResponseDate,
                            // Вытягиваем данные хоста
                            Host = new AuthorDto
                            {
                                FullName = src.Property.Host.FirstName + " " + src.Property.Host.LastName,
                                AvatarUrl = src.Property.Host.AvatarUrl
                            }
                        }
                ));
        }
    }
}