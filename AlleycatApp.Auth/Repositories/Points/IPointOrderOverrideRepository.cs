using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Points
{
    public interface IPointOrderOverrideRepository : ICrudRepository<PointOrderOverride, int>
    {
        Task<PointOrderOverride?> GetByPointAndUserIdAsync(int pointId, string userId);
    }
}
