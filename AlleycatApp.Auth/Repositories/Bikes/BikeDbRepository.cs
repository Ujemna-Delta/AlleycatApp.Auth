using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Bikes
{
    public class BikeDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<Bike, int>(context, mapper), IBikeRepository
    {
        public override IQueryable<Bike> Entities => context.Bikes;

        public async Task<IEnumerable<Bike>> GetBikesByUserIdAsync(string userId)
            => await Entities.Where(b => b.AttendeeId == userId).ToArrayAsync();

        public override async Task DeleteAsync(int id)
        {
            context.Bikes.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
