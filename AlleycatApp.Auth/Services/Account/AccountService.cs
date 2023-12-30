using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Account
{
    public class AccountService(UserManager<IdentityUser> userManager, IMapper mapper) : IAccountService
    {
        public async Task<IdentityResult> RegisterAsync(IdentityUser user, string password)
        {
            if (await userManager.FindByNameAsync(user.UserName ?? string.Empty) != null)
                return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "User with the specified name already exists." });

            return await userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(string userId, IdentityUser user)
        {
            var userToEdit = await userManager.FindByIdAsync(userId) ?? 
                             throw new InvalidOperationException("User with the given ID was not found.");

            if (await userManager.FindByNameAsync(user.UserName ?? string.Empty) != null)
                return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "User with the specified name already exists." });

            return await userManager.UpdateAsync(mapper.Map(user, userToEdit) ?? throw new InvalidOperationException("Invalid user mapping"));
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var userToEdit = await userManager.FindByIdAsync(userId) ??
                             throw new InvalidOperationException("User with the given ID was not found.");

            return await userManager.ChangePasswordAsync(userToEdit, currentPassword, newPassword);
        }

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var userToDelete = await userManager.FindByIdAsync(userId) ??
                             throw new InvalidOperationException("User with the given ID was not found.");

            return await userManager.DeleteAsync(userToDelete);
        }
    }
}
