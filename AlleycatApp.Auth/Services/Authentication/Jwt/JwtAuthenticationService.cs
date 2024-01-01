using System.Security.Claims;
using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public class JwtAuthenticationService(IUserServicesProvider userServicesProvider, IJwtTokenGenerator tokenGenerator) : IAuthenticationService
    {
        public async Task<SignInResult> SignInAsync(string username, string password)
        {
            var mgr = userServicesProvider.DefaultManager;
            var user = await mgr.FindByNameAsync(username);

            if (user == null || !await mgr.CheckPasswordAsync(user, password))
                return SignInResult.Failed;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange((await mgr.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));

            var token = tokenGenerator.GenerateToken(claims);
            return new JwtSignInResult { Token = tokenGenerator.SerializeToken(token) };
        }
    }
}
