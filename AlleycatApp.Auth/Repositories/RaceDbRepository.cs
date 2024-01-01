using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;

namespace AlleycatApp.Auth.Repositories
{
    public class RaceDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<Race, int>(context, mapper), IRaceRepository
    {
        public override IQueryable<Race> Entities => context.Races;

        public override async Task DeleteAsync(int id)
        {
            context.Races.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
