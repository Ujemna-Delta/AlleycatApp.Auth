using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Repositories;

namespace AlleycatApp.Auth.Infrastructure
{
    public class DataFactory
    {
        public static async Task InsertRacesAsync(IRaceRepository raceRepository, IEnumerable<Race> races)
        {
            if(raceRepository.Entities.Any())
                return;

            foreach (var race in races)
                await raceRepository.AddAsync(race);
        }

        public static IEnumerable<Race> GetSampleRaces() => new Race[]
        {
            new()
            {
                Id = 1,
                Name = "Sample race 1",
                BeginTime = new DateTime(2023, 12, 20),
                Description = "Sample description for race 1",
                IsActive = false,
                IsFreeOrder = false,
                StartAddress = "Sample address 1",
                ValueModifier = 1.4M
            },
            new()
            {
                Id = 2,
                Name = "Sample race 2",
                BeginTime = new DateTime(2023, 12, 7),
                Description = "Sample description for race 2",
                IsActive = true,
                IsFreeOrder = false,
                StartAddress = "Sample address 2",
                ValueModifier = 3M
            },
            new()
            {
                Id = 3,
                Name = "Sample race 3",
                BeginTime = new DateTime(2024, 1, 14),
                IsActive = false,
                IsFreeOrder = true,
                StartAddress = "Sample address 3",
            },
            new()
            {
                Id = 4,
                Name = "Sample race 4",
                BeginTime = new DateTime(2024, 3, 13),
                Description = "Sample description for race 4",
                IsActive = false,
                IsFreeOrder = true,
                StartAddress = "Sample address 4",
                ValueModifier = 2.7M
            },
            new()
            {
                Id = 5,
                Name = "Sample race 5",
                BeginTime = new DateTime(2023, 12, 29),
                Description = "Sample description for race 5",
                IsActive = false,
                IsFreeOrder = false,
                StartAddress = "Sample address 5",
            },
        };
    }
}
