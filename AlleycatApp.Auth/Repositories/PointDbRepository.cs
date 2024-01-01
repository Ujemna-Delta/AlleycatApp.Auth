using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories
{
    public class PointDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<Point, int>(context, mapper), IPointRepository
    {
        public override IQueryable<Point> Entities => context.Points;

        public override async Task DeleteAsync(int id)
        {
            context.Points.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Point>> GetByRaceIdAsync(int raceId) =>
            await Entities.Where(p => p.RaceId == raceId).ToArrayAsync();
    }
}
