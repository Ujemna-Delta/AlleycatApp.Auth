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
        }
    }
}
