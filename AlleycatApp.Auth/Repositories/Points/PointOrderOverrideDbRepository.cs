using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Points
{
    public class PointOrderOverrideDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<PointOrderOverride, int>(context, mapper), IPointOrderOverrideRepository
    {
        public override IQueryable<PointOrderOverride> Entities => context.PointOrderOverrides;

        public async Task<PointOrderOverride?> GetByPointAndUserIdAsync(int pointId, string userId) =>
            await Entities.SingleOrDefaultAsync(p => p.AttendeeId == userId && p.PointId == pointId);

        public override async Task DeleteAsync(int id)
        {
            context.PointOrderOverrides.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
