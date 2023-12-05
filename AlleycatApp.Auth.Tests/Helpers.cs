using AlleycatApp.Auth.Data;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Tests
{
    internal static class Helpers
    {
        public static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(dbName).Options;
            return new ApplicationDbContext(options);
        }
    }
}
