using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Points
{
    public class PointCompletionDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<PointCompletion, int>(context, mapper), IPointCompletionRepository
    {
        public override IQueryable<PointCompletion> Entities => context.PointCompletions;

        public async Task<IEnumerable<PointCompletion>> GetByUserIdAsync(string userId) =>
            await Entities.Where(p => p.AttendeeId == userId).ToArrayAsync();

        public async Task<IEnumerable<PointCompletion>> GetByPointIdAsync(int pointId) =>
            await Entities.Where(p => p.PointId == pointId).ToArrayAsync();

        public override async Task DeleteAsync(int id)
        {
            context.PointCompletions.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
