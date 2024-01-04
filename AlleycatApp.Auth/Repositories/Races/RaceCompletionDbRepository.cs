using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Races
{
    public class RaceCompletionDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<RaceCompletion, int>(context, mapper), IRaceCompletionRepository
    {
        public override IQueryable<RaceCompletion> Entities => context.RaceCompletions;

        public async Task<IEnumerable<RaceCompletion>> GetByUserId(string userId) =>
            await Entities.Where(r => r.AttendeeId == userId).ToArrayAsync();

        public async Task<IEnumerable<RaceCompletion>> GetByRaceId(int raceId) =>
            await Entities.Where(r => r.RaceId == raceId).ToArrayAsync();

        public override async Task DeleteAsync(int id)
        {
            context.RaceCompletions.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
