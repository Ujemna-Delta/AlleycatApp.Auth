using AlleycatApp.Auth.Models.Users;
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
    }
}
