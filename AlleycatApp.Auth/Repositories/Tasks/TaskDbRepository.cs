using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Tasks
{
    public class TaskDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<TaskModel, int>(context, mapper), ITaskRepository
    {
        public override IQueryable<TaskModel> Entities => context.Tasks;

        public async Task<IEnumerable<TaskModel>> GetByPointId(int pointId)
            => await Entities.Where(t => t.PointId == pointId).ToArrayAsync();

        public override async Task DeleteAsync(int id)
        {
            context.Tasks.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
