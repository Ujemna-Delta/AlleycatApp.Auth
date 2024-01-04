using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories.Races;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api.Completions
{
    [Route("api/completions/races")]
    [ApiController]
    public class RacesCompletionsController(IRaceCompletionRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRaceCompletions() =>
            Ok(await repository.Entities.Select(r => mapper.Map<RaceCompletionDto>(r)).ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaceCompletionById(int id)
        {
            var completion = await repository.FindByIdAsync(id);
            return completion != null ? Ok(completion) : NotFound();
        }

        [HttpGet("attendee/{id}")]
        public async Task<IActionResult> GetRaceCompletionsByUserId(string id) =>
            Ok(await repository.GetByUserIdAsync(id));

        [HttpGet("race/{id}")]
        public async Task<IActionResult> GetRaceCompletionsByRaceId(int id) =>
            Ok(await repository.GetByRaceIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> AddRaceCompletion(RaceCompletionDto raceCompletionDto)
        {
            try
            {
                var result = await repository.AddAsync(mapper.Map<RaceCompletion>(raceCompletionDto));
                return CreatedAtAction(nameof(AddRaceCompletion), mapper.Map<RaceCompletionDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRaceCompletion(int id, RaceCompletionDto raceCompletionDto)
        {
            try
            {
                await repository.UpdateAsync(id, mapper.Map<RaceCompletion>(raceCompletionDto));
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
        public async Task<IActionResult> DeleteRaceCompletion(int id)
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
