using System.Security.Claims;

namespace AlleycatApp.Auth.Services.Providers
{
    public interface IUserDataProvider
    {
        string? GetUserIdForClaimsPrincipal(ClaimsPrincipal principal);
    }
}
