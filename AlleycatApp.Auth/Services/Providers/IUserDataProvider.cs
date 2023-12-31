using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Providers
{
    public interface IUserDataProvider
    {
        string? GetUserIdForClaimsPrincipal(ClaimsPrincipal principal);
        string? GetRoleName<TUser>() where TUser : IdentityUser, new();
    }
}
