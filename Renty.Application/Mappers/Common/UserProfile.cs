using AutoMapper;
using Renty.Application.DTOs.GetProperties;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;


namespace Renty.Application.Mappers.Common
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, HostDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src =>
                    src.Languages != null && src.Languages.Any()
                        ? string.Join(", ", src.Languages.Select(l => l.Name))
                        : string.Empty))
                .ForMember(dest => dest.ResponseSpeed, opt => opt.MapFrom(src => src.ResponseSpeed ?? "Неизвестно"));
        }
    }
}
