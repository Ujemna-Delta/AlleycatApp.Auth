using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;

namespace AlleycatApp.Auth.Repositories
{
    public class LeagueDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<League, short>(context, mapper), ILeagueRepository
    {
        public override IQueryable<League> Entities => context.Leagues;

        public override async Task DeleteAsync(short id)
        {
            context.Leagues.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
