using AlleycatApp.Auth.Models.Validation;
using AlleycatApp.Auth.Services.Providers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Account
{
    public class AccountService(IUserServicesProvider userServicesProvider, IUserDataProvider userDataProvider, IMapper mapper) : IAccountService
    {
        public async Task<IdentityResult> RegisterAsync<TUser>(TUser user, string password) where TUser : IdentityUser, new()
        {
            ModelValidator.Validate(user);
            var mgr = userServicesProvider.ProvideManager<TUser>();

            if (await userServicesProvider.DefaultManager.FindByNameAsync(user.UserName ?? string.Empty) != null)
                return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "User with the specified name already exists." });

            var result = await mgr.CreateAsync(user, password);

            var roleName = userDataProvider.GetRoleName<TUser>();
            if (roleName != null && result.Succeeded)
                result = await mgr.AddToRoleAsync(user, roleName);

            return result;
        }

        public async Task<IdentityResult> UpdateAsync<TUser>(string userId, TUser user) where TUser : IdentityUser, new()
        {
            ModelValidator.Validate(user);
            var mgr = userServicesProvider.ProvideManager<TUser>();
            var userToEdit = await mgr.FindByIdAsync(userId) ?? 
                             throw new InvalidOperationException("User with the given ID was not found.");

            var existingUser = await userServicesProvider.DefaultManager.FindByNameAsync(user.UserName ?? string.Empty);
            if (existingUser != null && existingUser != userToEdit)
                return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "User with the specified name already exists." });

            return await mgr.UpdateAsync(mapper.Map(user, userToEdit) ?? throw new InvalidOperationException("Invalid user mapping"));
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var mgr = userServicesProvider.DefaultManager;
            var userToEdit = await mgr.FindByIdAsync(userId) ??
                             throw new InvalidOperationException("User with the given ID was not found.");

            return await mgr.ChangePasswordAsync(userToEdit, currentPassword, newPassword);
        }

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var mgr = userServicesProvider.DefaultManager;
            var userToDelete = await mgr.FindByIdAsync(userId) ??
                             throw new InvalidOperationException("User with the given ID was not found.");

            return await mgr.DeleteAsync(userToDelete);
        }
    }
}
