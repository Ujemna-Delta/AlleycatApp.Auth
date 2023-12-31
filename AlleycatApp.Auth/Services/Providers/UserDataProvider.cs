using System.Security.Claims;
using AlleycatApp.Auth.Infrastructure;
using AlleycatApp.Auth.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Providers
{
    public class UserDataProvider(IUserServicesProvider userServicesProvider) : IUserDataProvider
    {
        public string? GetUserIdForClaimsPrincipal(ClaimsPrincipal principal)
            => userServicesProvider.DefaultManager.GetUserId(principal);

        public string? GetRoleName<TUser>() where TUser : IdentityUser, new() => new TUser() switch
        {
            Manager => Constants.RoleNames.Manager,
            Pointer => Constants.RoleNames.Pointer,
            Attendee => Constants.RoleNames.Attendee,
            _ => null
        };

        public async Task<IEnumerable<string>> GetRolesForClaimsPrincipalAsync(ClaimsPrincipal principal)
        {
            var mgr = userServicesProvider.DefaultManager;
            var user = await mgr.FindByIdAsync(GetUserIdForClaimsPrincipal(principal) ?? string.Empty);

            if (user == null)
                return new List<string>();

            return await mgr.GetRolesAsync(user);
        }
    }
}
