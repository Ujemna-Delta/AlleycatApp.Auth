using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController(IPointRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPoints() =>
            Ok(await repository.Entities.Select(p => mapper.Map<PointDto>(p)).ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPointById(int id)
        {
            var point = await repository.FindByIdAsync(id);
            return point != null ? Ok(mapper.Map<PointDto>(point)) : NotFound();
        }

        [HttpGet("race/{id}")]
        public async Task<IActionResult> GetPointsByRaceId(int id) =>
            Ok((await repository.GetByRaceIdAsync(id)).Select(mapper.Map<PointDto>));

        [HttpPost]
        public async Task<IActionResult> AddPoint(PointDto pointDto)
        {
            try
            {
                var result = await repository.AddAsync(mapper.Map<Point>(pointDto));
                return CreatedAtAction(nameof(AddPoint), mapper.Map<PointDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoint(int id, PointDto pointDto)
        {
            try
            {
                await repository.UpdateAsync(id, mapper.Map<Point>(pointDto));
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
        public async Task<IActionResult> DeletePoint(int id)
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
