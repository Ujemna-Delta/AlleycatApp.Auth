using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Tasks
{
    public class TaskCompletionDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<TaskCompletion, int>(context, mapper), ITaskCompletionRepository
    {
        public override IQueryable<TaskCompletion> Entities => context.TaskCompletions;

        public async Task<IEnumerable<TaskCompletion>> GetByUserIdAsync(string userId) =>
            await Entities.Where(t => t.AttendeeId == userId).ToArrayAsync();

        public async Task<IEnumerable<TaskCompletion>> GetByTaskIdAsync(int taskId) =>
            await Entities.Where(t => t.TaskId == taskId).ToArrayAsync();

        public override async Task DeleteAsync(int id)
        {
            context.TaskCompletions.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
