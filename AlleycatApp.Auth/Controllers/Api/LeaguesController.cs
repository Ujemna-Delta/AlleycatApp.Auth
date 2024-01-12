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
    public class LeaguesController(ILeagueRepository repository, ILeagueScoreRepository scoreRepository, IMapper mapper) : ControllerBase
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

        [HttpGet("scores")]
        public async Task<IActionResult> GetLeagueScores() =>
            Ok((await scoreRepository.Entities.ToArrayAsync()).Select(mapper.Map<LeagueScoreDto>));

        [HttpGet("scores/{id}")]
        public async Task<IActionResult> GetLeagueScoreById(int id)
        {
            var score = await scoreRepository.FindByIdAsync(id);
            return score != null ? Ok(mapper.Map<LeagueScoreDto>(score)) : NotFound();
        }

        [HttpGet("scores/user/{userId}")]
        public async Task<IActionResult> GetLeagueScoresByUserId(string userId) =>
            Ok((await scoreRepository.GetByUserIdAsync(userId)).Select(mapper.Map<LeagueScoreDto>));

        [HttpGet("scores/league/{leagueId}")]
        public async Task<IActionResult> GetLeagueScoresByUserId(short leagueId) =>
            Ok((await scoreRepository.GetByLeagueIdAsync(leagueId)).Select(mapper.Map<LeagueScoreDto>));

        [HttpPost("scores")]
        public async Task<IActionResult> AddLeagueScore(LeagueScoreDto leagueScoreDto)
        {
            try
            {
                var score = mapper.Map<LeagueScore>(leagueScoreDto);
                var result = await scoreRepository.AddAsync(score);
                return CreatedAtAction(nameof(AddLeagueScore), mapper.Map<LeagueScoreDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("scores/{id}")]
        public async Task<IActionResult> UpdateLeagueScore(int id, LeagueScoreDto leagueScoreDto)
        {
            try
            {
                await scoreRepository.UpdateAsync(id, mapper.Map<LeagueScore>(leagueScoreDto));
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

        [HttpDelete("scores/{id}")]
        public async Task<IActionResult> DeleteLeagueScore(int id)
        {
            try
            {
                await scoreRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
