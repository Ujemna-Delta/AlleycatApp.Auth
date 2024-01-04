using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories.Leagues;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController(ILeagueRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetLeagues() =>
            Ok(await repository.Entities.Select(l => mapper.Map<LeagueDto>(l)).ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeagueById(short id)
        {
            var league = await repository.FindByIdAsync(id);
            return league != null ? Ok(mapper.Map<LeagueDto>(league)) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddLeague(LeagueDto leagueDto)
        {
            try
            {
                var result = await repository.AddAsync(mapper.Map<League>(leagueDto));
                return CreatedAtAction(nameof(AddLeague), mapper.Map<LeagueDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeague(short id, LeagueDto leagueDto)
        {
            try
            {
                await repository.UpdateAsync(id, mapper.Map<League>(leagueDto));
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
        public async Task<IActionResult> DeleteLeague(short id)
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
