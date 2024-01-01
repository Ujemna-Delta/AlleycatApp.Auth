using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController(IRaceRepository raceRepository, IMapper mapper) : ControllerBase
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
    }
}
