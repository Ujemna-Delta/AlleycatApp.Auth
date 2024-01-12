using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Leagues
{
    public class LeagueScoreDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<LeagueScore, int>(context, mapper), ILeagueScoreRepository
    {
        public override IQueryable<LeagueScore> Entities => context.LeagueScores;

        public async Task<IEnumerable<LeagueScore>> GetByUserIdAsync(string userId) =>
            await Entities.Where(l => l.AttendeeId == userId).ToArrayAsync();

        public async Task<IEnumerable<LeagueScore>> GetByLeagueIdAsync(short leagueId) =>
            await Entities.Where(l => l.LeagueId == leagueId).ToArrayAsync();

        public override async Task DeleteAsync(int id)
        {
            context.LeagueScores.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
