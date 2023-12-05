using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options);
}
