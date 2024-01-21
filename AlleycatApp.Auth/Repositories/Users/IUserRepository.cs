using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Repositories.Users
{
    public interface IUserRepository
    {
        IEnumerable<TUser> GetUsers<TUser>() where TUser : IdentityUser, new();
        Task<TUser?> FindByIdAsync<TUser>(string userId) where TUser : IdentityUser, new();
    }
}
