using AutoMapper;
using Renty.Application.DTOs.Booking;
using Renty.Domain.Models.LookupsTables;
using Renty.Web.Extensions;
using Renty.Web.Models.BookingModels;

namespace Renty.Web.Mappers.Booking
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingResultDto, BookingResultViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetDescription()))
                .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => src.Status == BookingStatusEnum.Confirmed));
        }
    }
}
