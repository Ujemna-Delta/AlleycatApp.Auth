using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public virtual DbSet<Manager> Managers => Set<Manager>();
        public virtual DbSet<Pointer> Pointers => Set<Pointer>();
        public virtual DbSet<Attendee> Attendees => Set<Attendee>();

        public virtual DbSet<League> Leagues => Set<League>();
        public virtual DbSet<Race> Races => Set<Race>();
        public virtual DbSet<Point> Points => Set<Point>();
        public virtual DbSet<TaskModel> Tasks => Set<TaskModel>();

        public virtual DbSet<Bike> Bikes => Set<Bike>();
        
        public virtual DbSet<RaceCompletion> RaceCompletions => Set<RaceCompletion>();
        public virtual DbSet<PointCompletion> PointCompletions => Set<PointCompletion>();
        public virtual DbSet<TaskCompletion> TaskCompletions => Set<TaskCompletion>();

        public virtual DbSet<RaceAttendance> RaceAttendances => Set<RaceAttendance>();
        public virtual DbSet<PointOrderOverride> PointOrderOverrides => Set<PointOrderOverride>();
        public virtual DbSet<LeagueScore> LeagueScores => Set<LeagueScore>();
    }
}
