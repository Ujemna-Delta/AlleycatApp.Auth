using AlleycatApp.Auth.Infrastructure;
using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Services.Account;
using AlleycatApp.Auth.Services.Providers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api.User
{
    [Route("api/users/[controller]")]
    [ApiController, Authorize]
    public class AccountController(IAccountService accountService, IUserDataProvider userDataProvider, IMapper mapper) : ControllerBase
    {
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> RegisterDefault(UserRegistrationDto<UserDto> userRegistrationDto)
            => await Register(mapper.Map<IdentityUser>(userRegistrationDto.User), userRegistrationDto.Password);

        [HttpPut]
        public async Task<IActionResult> UpdateDefault(UserDto userDto)
            => await Update(mapper.Map<IdentityUser>(userDto));

        [HttpPost("manager"), AllowAnonymous]
        public async Task<IActionResult> RegisterManager(UserRegistrationDto<ManagerDto> userRegistrationDto)
            => await Register(mapper.Map<Manager>(userRegistrationDto.User), userRegistrationDto.Password);

        [HttpPut("manager"), Authorize(Roles = Constants.RoleNames.Manager)]
        public async Task<IActionResult> UpdateManager(ManagerDto managerDto)
            => await Update(mapper.Map<Manager>(managerDto));

        [HttpPost("pointer"), AllowAnonymous]
        public async Task<IActionResult> RegisterPointer(UserRegistrationDto<PointerDto> userRegistrationDto)
            => await Register(mapper.Map<Pointer>(userRegistrationDto.User), userRegistrationDto.Password);

        [HttpPut("pointer"), Authorize(Roles = Constants.RoleNames.Pointer)]
        public async Task<IActionResult> UpdatePointer(PointerDto pointerDto)
            => await Update(mapper.Map<Pointer>(pointerDto));

        [HttpPost("attendee"), AllowAnonymous]
        public async Task<IActionResult> RegisterAttendee(UserRegistrationDto<AttendeeDto> userRegistrationDto)
            => await Register(mapper.Map<Attendee>(userRegistrationDto.User), userRegistrationDto.Password);

        [HttpPut("attendee"), Authorize(Roles = Constants.RoleNames.Attendee)]
        public async Task<IActionResult> UpdateAttendee(AttendeeDto attendeeDto)
            => await Update(mapper.Map<Attendee>(attendeeDto));

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
            try
            {
                var result = await accountService.RegisterAsync(user, password);
                return result.Succeeded ? Created() : BadRequest(result);
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        private async Task<IActionResult> Update<TUser>(TUser user) where TUser : IdentityUser, new()
        {
            try
            {
                var id = userDataProvider.GetUserIdForClaimsPrincipal(User);
                var result = await accountService.UpdateAsync(id ?? string.Empty, user);
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
