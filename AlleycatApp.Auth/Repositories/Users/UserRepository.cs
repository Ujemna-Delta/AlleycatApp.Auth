using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Repositories.Users
{
    public class UserRepository(IUserServicesProvider userServicesProvider) : IUserRepository
    {
        public IEnumerable<TUser> GetUsers<TUser>() where TUser : IdentityUser, new()
            => userServicesProvider.ProvideManager<TUser>().Users.ToArray();
    }
}
