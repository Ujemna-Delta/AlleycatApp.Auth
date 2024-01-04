using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Points
{
    public interface IPointCompletionRepository : ICrudRepository<PointCompletion, int>
    {
        Task<IEnumerable<PointCompletion>> GetByUserIdAsync(string userId);
        Task<IEnumerable<PointCompletion>> GetByPointIdAsync(int pointId);
    }
}
