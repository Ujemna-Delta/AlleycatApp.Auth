using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Leagues;
using AlleycatApp.Auth.Services.Account;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Infrastructure
{
    public static class DataFactory
    {
        public static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Constants.RoleNames.Manager));
            await roleManager.CreateAsync(new IdentityRole(Constants.RoleNames.Pointer));
            await roleManager.CreateAsync(new IdentityRole(Constants.RoleNames.Attendee));
        }

        public static async Task CreateInitialManager(IAccountService accountService, string userName, string password)
            => await accountService.RegisterAsync(new Manager { UserName = userName }, password);

        public static async Task SeedLeagues(ILeagueRepository repository)
        {
            if (!repository.Entities.Any())
            {
                await repository.AddAsync(new League { Name = "League 1", Description = "Description 1" });
                await repository.AddAsync(new League { Name = "League 2" });
                await repository.AddAsync(new League { Name = "League 3", Description = "Description 3" });
            }
        }
    }
}
