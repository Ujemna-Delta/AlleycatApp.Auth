using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories
{
    public interface IPointRepository : ICrudRepository<Point, int>
    {
        Task<IEnumerable<Point>> GetByRaceIdAsync(int raceId);
    }
}
