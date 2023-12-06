using AlleycatApp.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Race> Races => Set<Race>();
    }
}
