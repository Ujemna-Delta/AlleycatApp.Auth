using AlleycatApp.Auth.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Providers
{
    public class UserServicesProvider(UserManager<IdentityUser> defaultManager, UserManager<Manager> managersManager,
        UserManager<Pointer> pointersManager, UserManager<Attendee> attendeesManager) : IUserServicesProvider
    {
        public UserManager<IdentityUser> DefaultManager => defaultManager;

        public UserManager<TUser> ProvideManager<TUser>() where TUser : IdentityUser, new()
            => (UserManager<TUser>)GetManager<TUser>();

        private object GetManager<TUser>() where TUser : IdentityUser, new() => new TUser() switch
        {
            Manager => managersManager,
            Pointer => pointersManager,
            Attendee => attendeesManager,
            _ => defaultManager
        };
    }
}
