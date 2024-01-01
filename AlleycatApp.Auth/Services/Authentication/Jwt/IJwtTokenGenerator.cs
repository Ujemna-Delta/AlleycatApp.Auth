using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public interface IJwtTokenGenerator
    {
        SecurityToken GenerateToken(IEnumerable<Claim> claims);
        string SerializeToken(SecurityToken token);
    }
}
