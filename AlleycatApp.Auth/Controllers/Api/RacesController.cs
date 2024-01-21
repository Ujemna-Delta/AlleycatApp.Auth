using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Races;
using AlleycatApp.Auth.Repositories.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController(IRaceRepository raceRepository, IRaceAttendanceRepository raceAttendanceRepository, IMapper mapper, IUserRepository userRepository) : ControllerBase
    {
        [HttpGet] 
        public IActionResult GetRaces() => Ok(raceRepository.Entities.Select(e => mapper.Map<RaceDto>(e)).ToArray());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaceById(int id)
        {
            var race = await raceRepository.FindByIdAsync(id);
            return race != null ? Ok(mapper.Map<RaceDto>(race)) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddRace(RaceDto race)
        {
            try
            {
                var raceModel = mapper.Map<Race>(race);
                var createdRace = await raceRepository.AddAsync(raceModel);
                return CreatedAtAction(nameof(AddRace), mapper.Map<RaceDto>(createdRace));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRace(int id, RaceDto race)
        {
            try
            {
                var raceModel = mapper.Map<Race>(race);
                await raceRepository.UpdateAsync(id, raceModel);
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
        public async Task<IActionResult> DeleteRace(int id)
        {
            try
            {
                await raceRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpGet("attendances")]
        public async Task<IActionResult> GetAttendances() =>
            Ok((await raceAttendanceRepository.Entities.ToArrayAsync()).Select(mapper.Map<RaceAttendanceDto>));

        [HttpGet("attendances/{id}")]
        public async Task<IActionResult> GetAttendanceById(int id)
        {
            var attendance = await raceAttendanceRepository.FindByIdAsync(id);
            return attendance != null ? Ok(mapper.Map<RaceAttendanceDto>(attendance)) : NotFound();
        }

        [HttpGet("attendances/race/{id}")]
        public async Task<IActionResult> GetAttendeesForRace(int id)
        {
            var attendeeIds = (await raceAttendanceRepository.GetAttendancesForRaceAsync(id)).Select(r => r.AttendeeId);
            var users = attendeeIds.Select(a => mapper.Map<AttendeeDto>(userRepository.FindByIdAsync<Attendee>(a).Result));
            return Ok(users);
        }

        [HttpGet("attendances/race/count/{id}")]
        public async Task<IActionResult> GetAttendancesCountForRace(int id) =>
            Ok((await raceAttendanceRepository.GetAttendancesForRaceAsync(id)).Count().ToString());

        [HttpGet("attendances/{raceId}/{userId}")]
        public async Task<IActionResult> GetAttendanceByRaceAndUserId(int raceId, string userId)
        {
            var attendance = await raceAttendanceRepository.GetByUserAndRaceIdAsync(userId, raceId);
            return attendance != null ? Ok(mapper.Map<RaceAttendanceDto>(attendance)) : NotFound();
        }

        [HttpPost("attendances")]
        public async Task<IActionResult> AddAttendance(RaceAttendanceDto attendanceDto)
        {
            try
            {
                var attendance = mapper.Map<RaceAttendance>(attendanceDto);
                var result = await raceAttendanceRepository.AddAsync(attendance);
                return CreatedAtAction(nameof(AddAttendance), mapper.Map<RaceAttendanceDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("attendances/{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, RaceAttendanceDto attendanceDto)
        {
            try
            {
                await raceAttendanceRepository.UpdateAsync(id, mapper.Map<RaceAttendance>(attendanceDto));
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

        [HttpDelete("attendances/{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            try
            {
                await raceAttendanceRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
