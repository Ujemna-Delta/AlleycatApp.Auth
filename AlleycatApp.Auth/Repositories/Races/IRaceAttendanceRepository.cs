using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Races
{
    public interface IRaceAttendanceRepository : ICrudRepository<RaceAttendance, int>
    {
        Task<RaceAttendance?> GetByUserAndRaceIdAsync(string userId, int raceId);
    }
}
