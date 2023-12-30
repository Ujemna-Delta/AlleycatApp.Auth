using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<SignInResult> SignInAsync(string username, string password);
    }
}
