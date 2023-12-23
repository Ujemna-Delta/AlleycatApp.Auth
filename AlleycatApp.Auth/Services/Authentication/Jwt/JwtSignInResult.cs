using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public class JwtSignInResult : SignInResult
    {
        public string Token { get; init; } = null!;
    }
}
