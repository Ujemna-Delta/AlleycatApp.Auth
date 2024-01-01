using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Services.Authentication;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api.User
{
    [Route("api/users/[controller]")]
    [ApiController]
    public class SessionController(IAuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SignIn(UserCredentialsDto userCredentialsDto)
        {
            var result = await authenticationService.SignInAsync(userCredentialsDto.UserName, userCredentialsDto.Password);
            return result.Succeeded ? CreatedAtAction(nameof(SignIn), (result as JwtSignInResult)?.Token): BadRequest(result);
        }
    }
}
