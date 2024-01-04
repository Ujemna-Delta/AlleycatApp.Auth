using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Repositories.Points;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api.Completions
{
    [Route("api/completions/points")]
    [ApiController]
    public class PointCompletionsController(IPointCompletionRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPointCompletions() =>
            Ok(await repository.Entities.Select(r => mapper.Map<PointCompletionDto>(r)).ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPointCompletionById(int id)
        {
            var completion = await repository.FindByIdAsync(id);
            return completion != null ? Ok(mapper.Map<PointCompletionDto>(completion)) : NotFound();
        }

        [HttpGet("attendee/{id}")]
        public async Task<IActionResult> GetPointCompletionsByUserId(string id) =>
            Ok((await repository.GetByUserIdAsync(id)).Select(mapper.Map<PointCompletionDto>));

        [HttpGet("point/{id}")]
        public async Task<IActionResult> GetPointCompletionsByPointId(int id) =>
            Ok((await repository.GetByPointIdAsync(id)).Select(mapper.Map<PointCompletionDto>));

        [HttpPost]
        public async Task<IActionResult> AddPointCompletion(PointCompletionDto pointCompletionDto)
        {
            try
            {
                var result = await repository.AddAsync(mapper.Map<PointCompletion>(pointCompletionDto));
                return CreatedAtAction(nameof(AddPointCompletion), mapper.Map<PointCompletionDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePointCompletion(int id, PointCompletionDto pointCompletionDto)
        {
            try
            {
                await repository.UpdateAsync(id, mapper.Map<PointCompletion>(pointCompletionDto));
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
        public async Task<IActionResult> DeletePointCompletion(int id)
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
