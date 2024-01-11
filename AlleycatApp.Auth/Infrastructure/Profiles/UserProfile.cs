using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Infrastructure.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, IdentityUser>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.PasswordHash, opt => opt.Ignore());

            CreateMap<Manager, Manager>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.PasswordHash, opt => opt.Ignore());

            CreateMap<Pointer, Pointer>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.PasswordHash, opt => opt.Ignore());

            CreateMap<Attendee, Attendee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.PasswordHash, opt => opt.Ignore());

            CreateMap<UserDto, IdentityUser>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ManagerDto, Manager>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<PointerDto, Pointer>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<AttendeeDto, Attendee>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<IdentityUser, UserDto>();
            CreateMap<Manager, ManagerDto>();
            CreateMap<Pointer, PointerDto>();
            CreateMap<Attendee, AttendeeDto>();
        }
    }
}
