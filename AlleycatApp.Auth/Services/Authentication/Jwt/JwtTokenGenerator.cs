using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlleycatApp.Auth.Infrastructure.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public class JwtTokenGenerator(IApplicationConfigurationBuilder appConfigBuilder) : IJwtTokenGenerator
    {
        public SecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var jwtConfig = appConfigBuilder.BuildJwtConfiguration().Build().JwtConfiguration;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig!.SecretKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtConfig.Issuer,
                Audience = jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddMinutes(jwtConfig.ExpirationTimeMinutes),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public string SerializeToken(SecurityToken token) => new JwtSecurityTokenHandler().WriteToken(token);
    }
}
