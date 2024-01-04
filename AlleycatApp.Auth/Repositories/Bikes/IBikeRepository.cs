using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Bikes
{
    public interface IBikeRepository : ICrudRepository<Bike, int>
    {
        Task<IEnumerable<Bike>> GetBikesByUserIdAsync(string userId);
    }
}
