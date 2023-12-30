using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Providers
{
    public interface IUserServicesProvider
    {
        UserManager<IdentityUser> DefaultManager { get; }
        UserManager<TUser> ProvideManager<TUser>() where TUser : IdentityUser, new();
    }
}
