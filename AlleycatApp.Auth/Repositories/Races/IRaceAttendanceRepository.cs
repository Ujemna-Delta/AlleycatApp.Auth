using AlleycatApp.Auth.Models;

namespace AlleycatApp.Auth.Repositories.Races
{
    public interface IRaceAttendanceRepository : ICrudRepository<RaceAttendance, int>
    {
        Task<IEnumerable<RaceAttendance>> GetAttendancesForRaceAsync(int raceId);
        Task<RaceAttendance?> GetByUserAndRaceIdAsync(string userId, int raceId);
    }
}
