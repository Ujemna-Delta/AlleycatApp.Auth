using System.Security.Claims;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
