using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories
{
    public interface IRaceRepository : ICrudRepository<Race, int>
    {
        Task<Race?> FindByIdAsync(int id);
    }
}
