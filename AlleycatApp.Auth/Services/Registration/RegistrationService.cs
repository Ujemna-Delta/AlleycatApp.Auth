using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Registration
{
    public class RegistrationService(UserManager<IdentityUser> userManager, IMapper mapper) : IRegistrationService
    {
        public async Task<IdentityResult> RegisterAsync(IdentityUser user, string password)
        {
            if (await userManager.FindByNameAsync(user.UserName ?? string.Empty) == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "User with the specified name already exists." });

            return await userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser user)
        {
            var userToEdit = await userManager.FindByIdAsync(user.Id);

            if (await userManager.FindByNameAsync(user.UserName ?? string.Empty) == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "User with the specified name already exists." });

            return await userManager.UpdateAsync(mapper.Map(user, userToEdit) ?? throw new InvalidOperationException("Invalid user mapping"));
        }

        public async Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
            => await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var userToDelete = await userManager.FindByIdAsync(userId);

            if(userToDelete == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = $"User with ID {userId} not found" });

            return await userManager.DeleteAsync(userToDelete);
        }
    }
}
