using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Users;
using AlleycatApp.Auth.Repositories.Bikes;
using AlleycatApp.Auth.Repositories.Leagues;
using AlleycatApp.Auth.Repositories.Points;
using AlleycatApp.Auth.Repositories.Races;
using AlleycatApp.Auth.Repositories.Tasks;
using AlleycatApp.Auth.Repositories.Users;
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
            var accountService = serviceProvider.GetRequiredService<IAccountService>();
            var leagueRepository = serviceProvider.GetRequiredService<ILeagueRepository>();
            var raceRepository = serviceProvider.GetRequiredService<IRaceRepository>();
            var pointRepository = serviceProvider.GetRequiredService<IPointRepository>();
            var taskRepository = serviceProvider.GetRequiredService<ITaskRepository>();
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            var bikeRepository = serviceProvider.GetRequiredService<IBikeRepository>();

            await SeedLeagues(leagueRepository);
            await SeedRaces(raceRepository, leagueRepository);
            await SeedPoints(pointRepository, raceRepository);
            await SeedTasks(taskRepository, pointRepository);

            await SeedUsers(accountService, userRepository, pointRepository);

            await SeedBikes(bikeRepository, userRepository);
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
                    ValueModifier = 1.4M,
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
                    ValueModifier = 3.5M,
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

        private static async Task SeedPoints(IPointRepository pointRepository, IRaceRepository raceRepository)
        {
            if (!pointRepository.Entities.Any())
            {
                await pointRepository.AddAsync(new Point
                {
                    Name = "Point 1",
                    Address = "Address 1",
                    IsHidden = false,
                    IsPrepared = false,
                    Order = 1,
                    Race = raceRepository.Entities.FirstOrDefault(),
                    Value = 10
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Point 2",
                    Address = "Address 2",
                    IsHidden = false,
                    IsPrepared = false,
                    Order = 2,
                    Race = raceRepository.Entities.FirstOrDefault(),
                    Value = 5
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Point 3",
                    Address = "Address 3",
                    IsHidden = false,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.OrderBy(r => r.Id).LastOrDefault(),
                    Value = 20
                });
            }
        }

        private static async Task SeedTasks(ITaskRepository taskRepository, IPointRepository pointRepository)
        {
            if (!taskRepository.Entities.Any())
            {
                await taskRepository.AddAsync(new TaskModel
                {
                    Name = "Task 1",
                    Description = "Description 1",
                    Value = 15,
                    Point = pointRepository.Entities.FirstOrDefault()
                });

                await taskRepository.AddAsync(new TaskModel
                {
                    Name = "Task 2",
                    Value = 10,
                    Point = pointRepository.Entities.FirstOrDefault()
                });

                await taskRepository.AddAsync(new TaskModel
                {
                    Name = "Task 3",
                    Description = "Description 3",
                    Value = 5,
                    Point = pointRepository.Entities.Skip(1).FirstOrDefault()
                });
            }
        }

        private static async Task SeedUsers(IAccountService accountService, IUserRepository userRepository, IPointRepository pointRepository)
        {
            if (!userRepository.GetUsers<IdentityUser>().Any())
                await accountService.RegisterAsync(new IdentityUser { UserName = "user1" }, "S3cretP@ssword");

            if (!userRepository.GetUsers<Attendee>().Any())
            {
                await accountService.RegisterAsync(new Attendee
                {
                    UserName = "attendee1",
                    FirstName = "Attendee",
                    LastName = "First",
                    Nickname = "attendee1"
                }, "S3cretP@ssword");

                await accountService.RegisterAsync(new Attendee
                {
                    UserName = "attendee2",
                    FirstName = "Attendee",
                    LastName = "Second",
                    Nickname = "attendee2",
                    Marks = "Sample marks"
                }, "$3cretPassword");
            }

            if (!userRepository.GetUsers<Pointer>().Any())
            {
                await accountService.RegisterAsync(new Pointer
                {
                    UserName = "pointer1",
                    FirstName = "Pointer",
                    LastName = "First"
                }, "S3cretP@ssword");

                await accountService.RegisterAsync(new Pointer
                {
                    UserName = "pointer2",
                    FirstName = "Pointer",
                    LastName = "Second",
                    Point = pointRepository.Entities.FirstOrDefault()
                }, "$3cretPassword");
            }
        }

        private static async Task SeedBikes(IBikeRepository bikeRepository, IUserRepository userRepository)
        {
            if (!bikeRepository.Entities.Any())
            {
                await bikeRepository.AddAsync(new Bike { Name = "Bike 1", AttendeeId = userRepository.GetUsers<Attendee>().First().Id });
                await bikeRepository.AddAsync(new Bike { Name = "Bike 2", AttendeeId = userRepository.GetUsers<Attendee>().Skip(1).First().Id });
                await bikeRepository.AddAsync(new Bike { Name = "Bike 3", AttendeeId = userRepository.GetUsers<Attendee>().First().Id });
            }
        }
    }
}
