using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories.Points;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController(IPointRepository repository, IPointOrderOverrideRepository pointOrderRepository, IMapper mapper) : ControllerBase
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

        [HttpGet("orders/")]
        public async Task<IActionResult> GetPointOrderOverrides() =>
            Ok((await pointOrderRepository.Entities.ToArrayAsync()).Select(mapper.Map<PointOrderOverrideDto>));

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetPointOrderOverrideById(int id)
        {
            var order = await pointOrderRepository.FindByIdAsync(id);
            return order != null ? Ok(mapper.Map<PointOrderOverrideDto>(order)) : NotFound();
        }

        [HttpGet("orders/{pointId}/{userId}")]
        public async Task<IActionResult> GetPointOrderOverrideByPointAndUser(int pointId, string userId)
        {
            var order = await pointOrderRepository.GetByPointAndUserIdAsync(pointId, userId);
            return order != null ? Ok(mapper.Map<PointOrderOverrideDto>(order)) : NotFound();
        }

        [HttpPost("orders")]
        public async Task<IActionResult> AddPointOrderOverride(PointOrderOverrideDto orderDto)
        {
            try
            {
                var order = mapper.Map<PointOrderOverride>(orderDto);
                var result = await pointOrderRepository.AddAsync(order);
                return CreatedAtAction(nameof(AddPointOrderOverride), mapper.Map<PointOrderOverrideDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("orders/{id}")]
        public async Task<IActionResult> EditPointOrderOverride(int id, PointOrderOverrideDto orderDto)
        {
            try
            {
                await pointOrderRepository.UpdateAsync(id, mapper.Map<PointOrderOverride>(orderDto));
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

        [HttpDelete("orders/{id}")]
        public async Task<IActionResult> DeletePointOrderOverride(int id)
        {
            try
            {
                await pointOrderRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
