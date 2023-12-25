using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Services.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api.User
{
    [Route("api/users/[controller]")]
    [ApiController]
    public class AccountController(IRegistrationService registrationService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var result = await registrationService.RegisterAsync(new IdentityUser { UserName = userDto.UserName }, userDto.Password);
            return result.Succeeded ? Created() : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserDto userDto)
        {
            try
            {
                var result = await registrationService.UpdateAsync(id, new IdentityUser { UserName = userDto.UserName });
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("password/{id}")]
        public async Task<IActionResult> ChangePassword(string id, string currentPassword, string newPassword)
        {
            try
            {
                var result = await registrationService.ChangePasswordAsync(id, currentPassword, newPassword);
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await registrationService.DeleteAsync(id);
                return result.Succeeded ? NoContent() : BadRequest(result);
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }

        }
    }
}
