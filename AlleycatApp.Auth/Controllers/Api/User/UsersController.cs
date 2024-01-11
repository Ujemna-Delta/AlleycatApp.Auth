using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserRepository userRepository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers() => Ok(userRepository.GetUsers<IdentityUser>().Select(mapper.Map<UserDto>));

        [HttpGet("attendees")]
        public IActionResult GetAttendees() => Ok(userRepository.GetUsers<Attendee>().Select(mapper.Map<AttendeeDto>));

        [HttpGet("pointers")]
        public IActionResult GetPointers() => Ok(userRepository.GetUsers<Pointer>().Select(mapper.Map<PointerDto>));

        [HttpGet("managers")]
        public IActionResult GetManagers() => Ok(userRepository.GetUsers<Manager>().Select(mapper.Map<ManagerDto>));
    }
}
