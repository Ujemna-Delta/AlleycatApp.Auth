using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Services.Account;
using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api.User
{
    [Route("api/users/[controller]")]
    [ApiController, Authorize]
    public class AccountController(IAccountService accountService, IUserDataProvider userDataProvider) : ControllerBase
    {
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var result = await accountService.RegisterAsync(new IdentityUser { UserName = userDto.UserName }, userDto.Password);
            return result.Succeeded ? Created() : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            try
            {
                var id = userDataProvider.GetUserIdForClaimsPrincipal(User);
                var result = await accountService.UpdateAsync(id ?? string.Empty, new IdentityUser { UserName = userDto.UserName });
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeDto passwordChangeDto)
        {
            try
            {
                var id = userDataProvider.GetUserIdForClaimsPrincipal(User);
                var result = await accountService.ChangePasswordAsync(id ?? string.Empty, passwordChangeDto.CurrentPassword, passwordChangeDto.NewPassword);
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var id = userDataProvider.GetUserIdForClaimsPrincipal(User);
                var result = await accountService.DeleteAsync(id ?? string.Empty);
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
