using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Repositories.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(ITaskRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTasks() =>
            Ok(await repository.Entities.Select(t => mapper.Map<TaskDto>(t)).ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await repository.FindByIdAsync(id);
            return task != null ? Ok(mapper.Map<TaskDto>(task)) : NotFound();
        }

        [HttpGet("point/{id}")]
        public async Task<IActionResult> GetTasksByPointId(int id) =>
            Ok((await repository.GetByPointId(id)).Select(mapper.Map<TaskDto>));

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDto taskDto)
        {
            try
            {
                var result = await repository.AddAsync(mapper.Map<TaskModel>(taskDto));
                return CreatedAtAction(nameof(AddTask), mapper.Map<TaskDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDto taskDto)
        {
            try
            {
                await repository.UpdateAsync(id, mapper.Map<TaskModel>(taskDto));
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
        public async Task<IActionResult> DeleteTask(int id)
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
