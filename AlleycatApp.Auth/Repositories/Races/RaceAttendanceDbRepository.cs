using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories.Races
{
    public class RaceAttendanceDbRepository(ApplicationDbContext context, IMapper mapper) : CrudDbRepository<RaceAttendance, int>(context, mapper), IRaceAttendanceRepository
    {
        public override IQueryable<RaceAttendance> Entities => context.RaceAttendances;

        public async Task<RaceAttendance?> GetByUserAndRaceIdAsync(string userId, int raceId) =>
            await Entities.SingleOrDefaultAsync(r => r.AttendeeId == userId && r.RaceId == raceId);

        public override async Task DeleteAsync(int id)
        {
            context.RaceAttendances.Remove(await FindByIdStrictAsync(id));
            await context.SaveChangesAsync();
        }
    }
}
