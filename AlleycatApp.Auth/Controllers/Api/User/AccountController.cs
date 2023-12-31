using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Models.Users;
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
        public async Task<IActionResult> RegisterDefault(UserRegistrationDto<UserDto> userRegistrationDto)
            => await Register(new IdentityUser { UserName = userRegistrationDto.User.UserName }, userRegistrationDto.Password);

        [HttpPut]
        public async Task<IActionResult> UpdateDefault(UserDto userDto)
            => await Update(new IdentityUser { UserName = userDto.UserName });

        [HttpPost("manager"), AllowAnonymous]
        public async Task<IActionResult> RegisterManager(UserRegistrationDto<ManagerDto> userRegistrationDto)
            => await Register(new Manager { UserName = userRegistrationDto.User.UserName }, userRegistrationDto.Password);

        [HttpPut("manager")]
        public async Task<IActionResult> UpdateManager(ManagerDto managerDto)
            => await Update(new Manager { UserName = managerDto.UserName });

        [HttpPost("pointer"), AllowAnonymous]
        public async Task<IActionResult> RegisterPointer(UserRegistrationDto<PointerDto> userRegistrationDto)
            => await Register(new Pointer { UserName = userRegistrationDto.User.UserName }, userRegistrationDto.Password);

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

        private async Task<IActionResult> Register<TUser>(TUser user, string password) where TUser : IdentityUser, new()
        {
            var result = await accountService.RegisterAsync(user, password);
            return result.Succeeded ? Created() : BadRequest(result);
        }

        private async Task<IActionResult> Update<TUser>(TUser user) where TUser : IdentityUser, new()
        {
            try
            {
                var id = userDataProvider.GetUserIdForClaimsPrincipal(User);
                var result = await accountService.UpdateAsync(id ?? string.Empty, user);
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
