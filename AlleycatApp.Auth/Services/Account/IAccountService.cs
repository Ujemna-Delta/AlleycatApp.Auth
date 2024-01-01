using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Account
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync<TUser>(TUser user, string password) where TUser : IdentityUser, new();
        Task<IdentityResult> UpdateAsync<TUser>(string userId, TUser user) where TUser : IdentityUser, new();
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> DeleteAsync(string userId);
    }
}
