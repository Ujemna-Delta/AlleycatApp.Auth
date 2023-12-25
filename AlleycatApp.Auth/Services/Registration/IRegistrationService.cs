using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Registration
{
    public interface IRegistrationService
    {
        Task<IdentityResult> RegisterAsync(IdentityUser user, string password);
        Task<IdentityResult> UpdateAsync(string userId, IdentityUser user);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> DeleteAsync(string userId);
    }
}
