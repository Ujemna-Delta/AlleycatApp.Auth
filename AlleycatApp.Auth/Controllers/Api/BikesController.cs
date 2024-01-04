using AlleycatApp.Auth.Infrastructure;
using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories.Bikes;
using AlleycatApp.Auth.Services.Providers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = Constants.RoleNames.Attendee)]
    public class BikesController(IBikeRepository repository, IUserDataProvider userDataProvider, IMapper mapper) : ControllerBase
    {
        private string UserId => userDataProvider.GetUserIdForClaimsPrincipal(User) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetBikes()
            => Ok(await repository.GetBikesByUserIdAsync(UserId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBikeById(int id)
        {
            var result = await repository.GetBikeByUserAndIdAsync(UserId, id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddBike(BikeDto bikeDto)
        {
            try
            {
                var bike = mapper.Map<Bike>(bikeDto);
                bike.AttendeeId = UserId;
                var result = await repository.AddAsync(bike);
                return CreatedAtAction(nameof(AddBike), mapper.Map<BikeDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBike(int id, BikeDto bikeDto)
        {
            try
            {
                var bike = mapper.Map<Bike>(bikeDto);
                bike.AttendeeId = UserId;
                await repository.UpdateAsync(id, bike);
                return NoContent();
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            try
            {
                await repository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
