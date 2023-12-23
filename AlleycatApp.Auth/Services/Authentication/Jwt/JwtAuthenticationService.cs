using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public class JwtAuthenticationService(UserManager<IdentityUser> userManager, IJwtTokenGenerator tokenGenerator) : IAuthenticationService
    {
        public async Task<SignInResult> SignInAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
                return SignInResult.Failed;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = tokenGenerator.GenerateToken(claims);
            return new JwtSignInResult { Token = token };
        }
    }
}
