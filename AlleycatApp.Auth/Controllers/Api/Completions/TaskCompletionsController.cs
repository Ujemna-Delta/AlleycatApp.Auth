using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Repositories.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Controllers.Api.Completions
{
    [Route("api/completions/tasks")]
    [ApiController]
    public class TaskCompletionsController(ITaskCompletionRepository repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTaskCompletions() =>
            Ok(await repository.Entities.Select(r => mapper.Map<TaskCompletionDto>(r)).ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskCompletionById(int id)
        {
            var completion = await repository.FindByIdAsync(id);
            return completion != null ? Ok(mapper.Map<TaskCompletionDto>(completion)) : NotFound();
        }

        [HttpGet("attendee/{id}")]
        public async Task<IActionResult> GetTaskCompletionsByUserId(string id) =>
            Ok((await repository.GetByUserIdAsync(id)).Select(mapper.Map<TaskCompletionDto>));

        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTaskCompletionsByTaskId(int id) =>
            Ok((await repository.GetByTaskIdAsync(id)).Select(mapper.Map<TaskCompletionDto>));

        [HttpPost]
        public async Task<IActionResult> AddTaskCompletion(TaskCompletionDto taskCompletionDto)
        {
            try
            {
                var result = await repository.AddAsync(mapper.Map<TaskCompletion>(taskCompletionDto));
                return CreatedAtAction(nameof(AddTaskCompletion), mapper.Map<TaskCompletionDto>(result));
            }
            catch (InvalidModelException e)
            {
                return BadRequest(e.ModelError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskCompletion(int id, TaskCompletionDto taskCompletionDto)
        {
            try
            {
                await repository.UpdateAsync(id, mapper.Map<TaskCompletion>(taskCompletionDto));
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
        public async Task<IActionResult> DeleteTaskCompletion(int id)
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
