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

        public virtual DbSet<Race> Races => Set<Race>();
    }
}
