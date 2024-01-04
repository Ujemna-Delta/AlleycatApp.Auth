using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Races
{
    public interface IRaceCompletionRepository : ICrudRepository<RaceCompletion, int>
    {
        Task<IEnumerable<RaceCompletion>> GetByUserId(string userId);
        Task<IEnumerable<RaceCompletion>> GetByRaceId(int raceId);
    }
}
