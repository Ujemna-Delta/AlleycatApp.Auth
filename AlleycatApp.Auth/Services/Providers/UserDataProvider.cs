using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Providers
{
    public class UserDataProvider(UserManager<IdentityUser> userManager) : IUserDataProvider
    {
        public string? GetUserIdForClaimsPrincipal(ClaimsPrincipal principal)
            => userManager.GetUserId(principal);
    }
}
