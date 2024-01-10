using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Leagues;
using AlleycatApp.Auth.Repositories.Races;
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

        public static async Task SeedSampleData(IServiceProvider serviceProvider)
        {
            var leagueRepository = serviceProvider.GetRequiredService<ILeagueRepository>();
            var raceRepository = serviceProvider.GetRequiredService<IRaceRepository>();

            await SeedLeagues(leagueRepository);
            await SeedRaces(raceRepository, leagueRepository);
        }

        private static async Task SeedLeagues(ILeagueRepository repository)
        {
            if (!repository.Entities.Any())
            {
                await repository.AddAsync(new League { Name = "League 1", Description = "Description 1" });
                await repository.AddAsync(new League { Name = "League 2" });
                await repository.AddAsync(new League { Name = "League 3", Description = "Description 3" });
            }
        }

        private static async Task SeedRaces(IRaceRepository repository, ILeagueRepository leagueRepository)
        {
            if (!repository.Entities.Any())
            {
                await repository.AddAsync(new Race
                {
                    Name = "Race 1", 
                    BeginTime = new DateTime(2024, 1, 10, 12, 30, 0), 
                    Description = "Description 1",
                    IsActive = false, 
                    IsFreeOrder = true, 
                    StartAddress = "Address 1",
                    League = leagueRepository.Entities.FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Race 2",
                    BeginTime = new DateTime(2024, 1, 11, 15, 0, 0),
                    IsActive = false,
                    IsFreeOrder = false,
                    StartAddress = "Address 2",
                    League = leagueRepository.Entities.OrderBy(l => l.Id).LastOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Race 3",
                    BeginTime = new DateTime(2024, 1, 9, 10, 45, 0),
                    Description = "Description 3",
                    IsActive = true,
                    IsFreeOrder = true,
                    StartAddress = "Address 3",
                    League = leagueRepository.Entities.FirstOrDefault()
                });
            }
        }
    }
}
