using AlleycatApp.Auth.Data;
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

        public static async Task ClearDatabase(ApplicationDbContext context)
        {
            context.LeagueScores.RemoveRange(context.LeagueScores);
            context.PointOrderOverrides.RemoveRange(context.PointOrderOverrides);
            context.RaceAttendances.RemoveRange(context.RaceAttendances);

            context.TaskCompletions.RemoveRange(context.TaskCompletions);
            context.PointCompletions.RemoveRange(context.PointCompletions);
            context.RaceCompletions.RemoveRange(context.RaceCompletions);

            context.Bikes.RemoveRange(context.Bikes);
            context.Tasks.RemoveRange(context.Tasks);
            context.Points.RemoveRange(context.Points);
            context.Races.RemoveRange(context.Races);
            context.Leagues.RemoveRange(context.Leagues);

            context.Attendees.RemoveRange(context.Attendees);
            context.Pointers.RemoveRange(context.Pointers);

            await context.SaveChangesAsync();
        }

        public static async Task SeedSampleData(IServiceProvider serviceProvider)
        {
            var accountService = serviceProvider.GetRequiredService<IAccountService>();
            var leagueRepository = serviceProvider.GetRequiredService<ILeagueRepository>();
            var raceRepository = serviceProvider.GetRequiredService<IRaceRepository>();
            var pointRepository = serviceProvider.GetRequiredService<IPointRepository>();
            var taskRepository = serviceProvider.GetRequiredService<ITaskRepository>();
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            var bikeRepository = serviceProvider.GetRequiredService<IBikeRepository>();
            var leagueScoreRepository = serviceProvider.GetRequiredService<ILeagueScoreRepository>();
            var raceAttendanceRepository = serviceProvider.GetRequiredService<IRaceAttendanceRepository>();

            await SeedLeagues(leagueRepository);
            await SeedRaces(raceRepository, leagueRepository);
            await SeedPoints(pointRepository, raceRepository);
            await SeedTasks(taskRepository, pointRepository);
            await SeedUsers(accountService, userRepository, pointRepository);
            await SeedBikes(bikeRepository, userRepository);
            await SeedRaceAttendances(raceAttendanceRepository, raceRepository, userRepository);
            await SeedLeagueScores(leagueScoreRepository, leagueRepository, userRepository);
        }

        private static async Task SeedLeagues(ILeagueRepository repository)
        {
            if (!repository.Entities.Any())
            {
                await repository.AddAsync(new League { Name = "Metropolitalna Liga Kolarska", Description = "Rywalizuj w serii ekscytujących wyścigów, odkrywając uroki miasta na każdym kilometrze trasy. Dołącz do elity rowerowej!" });
                await repository.AddAsync(new League { Name = "Miejska Liga Kolarska", Description = "Zawody na najwyższym poziomie, łączące pasję kolarstwa z fascynującą atmosferą metropolii. Dołącz do ligi, gdzie triumfujesz na każdym etapie!" });
                await repository.AddAsync(new League { Name = "Liga Rowerowa WarsawTour", Description = "Przemierzaj miejskie trasy z pasją i ambicją, uczestnicząc w cyklu wyścigów, które przynoszą niezapomniane przeżycia kolarskim entuzjastom." });
            }
        }

        private static async Task SeedRaces(IRaceRepository repository, ILeagueRepository leagueRepository)
        {
            if (!repository.Entities.Any())
            {
                await repository.AddAsync(new Race
                {
                    Name = "Wyścig Kolarski Klasyków", 
                    BeginTime = new DateTime(2024, 1, 10, 12, 30, 0), 
                    Description = "Zawodnicy mierzą się na ulicach stolicy, w szybkim i widowiskowym wyścigu na dystansie miejskim.",
                    IsActive = false, 
                    IsFreeOrder = true, 
                    ValueModifier = 1.4M,
                    StartAddress = "Marszałkowska 10, Warszawa",
                    League = leagueRepository.Entities.FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Puchar Kolarski ProTour",
                    BeginTime = new DateTime(2024, 1, 11, 15, 0, 0),
                    Description = "Wyścig Kolarski ProTour to widowisko, gdzie elita kolarstwa ściga się na najwyższym poziomie, tocząc zaciętą walkę o zwycięstwo.",
                    IsActive = false,
                    IsFreeOrder = false,
                    StartAddress = "Nowy Świat 22, Warszawa",
                    ValueModifier = 3.5M,
                    League = leagueRepository.Entities.Skip(1).FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Klasyczny Maraton Rowerowy",
                    BeginTime = new DateTime(2024, 1, 9, 10, 45, 0),
                    Description = " Przez brzegi Wisły, uczestnicy pokonują wyjątkowe etapy, eksplorując uroki Warszawy nad rzeką.",
                    IsActive = false,
                    IsFreeOrder = true,
                    ValueModifier = 2.5M,
                    StartAddress = "Krakowskie Przedmieście 5, Warszawa",
                    League = leagueRepository.Entities.FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Grand Prix Kolarskiej Elity",
                    BeginTime = new DateTime(2024, 3, 13, 10, 45, 0),
                    Description = " Zawodnicy ścigają się wzdłuż ikonicznych alei miasta, tworząc dynamiczną trasę i przyciągając tłumy.",
                    IsActive = true,
                    IsFreeOrder = true,
                    StartAddress = "Aleje Jerozolimskie 65/79, Warszawa",
                    League = leagueRepository.Entities.Skip(2).FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Wyścig Mistrzów Rowerowych",
                    BeginTime = new DateTime(2024, 4, 4, 10, 45, 0),
                    Description = "Prestiżowy wyścig, który wiedzie przez zabytkowe arterie miasta, podkreślając piękno Warszawy w ruchu kolarskim.",
                    IsActive = false,
                    IsFreeOrder = false,
                    ValueModifier = 1.5M,
                    StartAddress = "Plac Zamkowy 4, Warszawa",
                    League = leagueRepository.Entities.Skip(1).FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Kolarski Wyścig Grand Slam",
                    BeginTime = new DateTime(2024, 3, 25, 10, 45, 0),
                    Description = "Trasa biegnie przez kultowe dzielnice, zapewniając widowiskowe ściganie na tle historycznych zakątków Warszawy.",
                    IsActive = false,
                    IsFreeOrder = false,
                    StartAddress = "Chmielna 21, Warszawa",
                    League = leagueRepository.Entities.FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Runda Kolarska Pro Challenge",
                    BeginTime = new DateTime(2024, 2, 19, 10, 45, 0),
                    Description = "Trasa prowadzi wzdłuż Wisły, oferując kolarzom zarówno piękne widoki, jak i trudne wyzwania terenowe.",
                    IsActive = false,
                    IsFreeOrder = true,
                    ValueModifier = 1.5M,
                    StartAddress = "Foksal 16, Warszawa",
                    League = leagueRepository.Entities.Skip(2).FirstOrDefault()
                });

                await repository.AddAsync(new Race
                {
                    Name = "Rowerowy Wyścig Wzgórz",
                    BeginTime = new DateTime(2024, 2, 9, 10, 45, 0),
                    Description = "Trasa wokół Warszawy wiedzie przez malownicze wzgórza, sprawiając, że wyścig staje się wyjątkowym przeżyciem.",
                    IsActive = false,
                    IsFreeOrder = false,
                    StartAddress = "Łazienki Królewskie, Warszawa",
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
                    Name = "Punkt Alfa",
                    Address = "Aleje Jerozolimskie 123, Warszawa",
                    IsHidden = false,
                    IsPrepared = false,
                    Order = 1,
                    Race = raceRepository.Entities.FirstOrDefault(),
                    Value = 10
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Beta",
                    Address = "Nowy Świat 45, Warszawa",
                    IsHidden = false,
                    IsPrepared = false,
                    Order = 2,
                    Race = raceRepository.Entities.FirstOrDefault(),
                    Value = 5
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Gamma",
                    Address = "ul. Marszałkowska 67/89, Warszawa",
                    IsHidden = false,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(2).FirstOrDefault(),
                    Value = 8
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Delta",
                    Address = "Plac Trzech Krzyży 11, Warszawa",
                    IsHidden = false,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(2).FirstOrDefault(),
                    Value = 13
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Epsilon",
                    Address = "Krakowskie Przedmieście 22, Warszawa",
                    IsHidden = false,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(3).FirstOrDefault(),
                    Value = 4
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Dzeta",
                    Address = "Foksal 55, Warszawa",
                    IsHidden = false,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(4).FirstOrDefault(),
                    Value = 16
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Eta",
                    Address = "Chmielna 34, Warszawa",
                    IsHidden = true,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(4).FirstOrDefault(),
                    Value = 33
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Theta",
                    Address = "Łazienki Królewskie, Warszawa",
                    IsHidden = false,
                    IsPrepared = false,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(5).FirstOrDefault(),
                    Value = 11
                });

                await pointRepository.AddAsync(new Point
                {
                    Name = "Punkt Iota",
                    Address = "Wilanowska 78, Warszawa",
                    IsHidden = false,
                    IsPrepared = true,
                    Order = 1,
                    Race = raceRepository.Entities.Skip(6).FirstOrDefault(),
                    Value = 27
                });
            }
        }

        private static async Task SeedTasks(ITaskRepository taskRepository, IPointRepository pointRepository)
        {
            if (!taskRepository.Entities.Any())
            {
                await taskRepository.AddAsync(new TaskModel
                {
                    Name = "Wrzucić piłkę do kosza",
                    Description = "Rzut ma być wykonany z dwutaktu",
                    Value = 15,
                    Point = pointRepository.Entities.Skip(6).FirstOrDefault()
                });

                await taskRepository.AddAsync(new TaskModel
                {
                    Name = "Zrobić 15 pompek",
                    Value = 10,
                    Point = pointRepository.Entities.Skip(7).FirstOrDefault()
                });

                await taskRepository.AddAsync(new TaskModel
                {
                    Name = "Przywieźć jajko w nienaruszonym stanie",
                    Description = "Zadanie zostanie niezaliczone, kiedy jajko nie zostanie dostarczone lub będzie uszkodzone",
                    Value = 35,
                    Point = pointRepository.Entities.Skip(5).FirstOrDefault()
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

                await accountService.RegisterAsync(new Attendee
                {
                    UserName = "attendee3",
                    FirstName = "Attendee",
                    LastName = "Third",
                    Nickname = "attendee3",
                    Marks = "Sample marks"
                }, "$3cretPassword");

                await accountService.RegisterAsync(new Attendee
                {
                    UserName = "attendee4",
                    FirstName = "Attendee",
                    LastName = "Fourth",
                    Nickname = "attendee4",
                    Marks = "Sample marks"
                }, "$3cretPassword");

                await accountService.RegisterAsync(new Attendee
                {
                    UserName = "attendee5",
                    FirstName = "Attendee",
                    LastName = "Fifth",
                    Nickname = "attendee5",
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

        private static async Task SeedRaceAttendances(IRaceAttendanceRepository raceAttendanceRepository,
            IRaceRepository raceRepository, IUserRepository userRepository)
        {
            if (!raceAttendanceRepository.Entities.Any())
            {
                await raceAttendanceRepository.AddAsync(new RaceAttendance
                {
                    IsConfirmed = true,
                    AttendeeId = userRepository.GetUsers<Attendee>().First().Id,
                    Race = raceRepository.Entities.Skip(4).First()
                });

                await raceAttendanceRepository.AddAsync(new RaceAttendance
                {
                    IsConfirmed = true,
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(1).First().Id,
                    Race = raceRepository.Entities.Skip(4).First()
                });

                await raceAttendanceRepository.AddAsync(new RaceAttendance
                {
                    IsConfirmed = true,
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(2).First().Id,
                    Race = raceRepository.Entities.Skip(4).First()
                });

                await raceAttendanceRepository.AddAsync(new RaceAttendance
                {
                    IsConfirmed = true,
                    AttendeeId = userRepository.GetUsers<Attendee>().First().Id,
                    Race = raceRepository.Entities.Skip(5).First()
                });

                await raceAttendanceRepository.AddAsync(new RaceAttendance
                {
                    IsConfirmed = true,
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(1).First().Id,
                    Race = raceRepository.Entities.Skip(5).First()
                });

                await raceAttendanceRepository.AddAsync(new RaceAttendance
                {
                    IsConfirmed = true,
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(2).First().Id,
                    Race = raceRepository.Entities.Skip(6).First()
                });
            }
        }

        private static async Task SeedLeagueScores(ILeagueScoreRepository leagueScoreRepository, ILeagueRepository leagueRepository, IUserRepository userRepository)
        {
            if (!leagueScoreRepository.Entities.Any())
            {
                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().First().Id,
                    League = leagueRepository.Entities.First(),
                    Score = 1000
                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(1).First().Id,
                    League = leagueRepository.Entities.First(),
                    Score = 1200
                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(2).First().Id,
                    League = leagueRepository.Entities.First(),
                    Score = 1400
                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(3).First().Id,
                    League = leagueRepository.Entities.First(),
                    Score = 1600
                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().First().Id,
                    League = leagueRepository.Entities.Skip(1).First(),
                    Score = 1400

                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(1).First().Id,
                    League = leagueRepository.Entities.Skip(1).First(),
                    Score = 1200
                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(2).First().Id,
                    League = leagueRepository.Entities.Skip(1).First(),
                    Score = 1350
                });

                await leagueScoreRepository.AddAsync(new LeagueScore
                {
                    AttendeeId = userRepository.GetUsers<Attendee>().Skip(3).First().Id,
                    League = leagueRepository.Entities.Skip(1).First(),
                    Score = 1600
                });
            }
        }
    }
}
