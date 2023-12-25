using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Authentication.Jwt
{
    public class JwtSignInResult : SignInResult
    {
        private readonly string _jwtToken = null!;

        public string Token
        {
            get => _jwtToken;
            init
            {
                Succeeded = true;
                _jwtToken = value;
            }
        }
    }
}
