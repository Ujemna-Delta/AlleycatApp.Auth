using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Points
{
    public interface IPointRepository : ICrudRepository<Point, int>
    {
        Task<IEnumerable<Point>> GetByRaceIdAsync(int raceId);
    }
}
