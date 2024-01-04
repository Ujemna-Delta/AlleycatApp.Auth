using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Races
{
    public interface IRaceCompletionRepository : ICrudRepository<RaceCompletion, int>
    {
        Task<IEnumerable<RaceCompletion>> GetByUserIdAsync(string userId);
        Task<IEnumerable<RaceCompletion>> GetByRaceIdAsync(int raceId);
    }
}
