using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Leagues
{
    public interface ILeagueScoreRepository : ICrudRepository<LeagueScore, int>
    {
        Task<IEnumerable<LeagueScore>> GetByUserIdAsync(string userId);
        Task<IEnumerable<LeagueScore>> GetByLeagueIdAsync(short leagueId);
    }
}
