using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Repositories;

namespace AlleycatApp.Auth.Tests.Repositories
{
    public class RaceDbRepositoryTests
    {
        [Fact]
        public void CanQueryObjects()
        {
            // Arrange

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
            };

            var context = Helpers.CreateInMemoryContext("CanQueryRaces_Db");
            var repository = new RaceDbRepository(context);

            context.Races.AddRange(races);
            context.SaveChanges();

            // Act

            var queriedRaces = repository.Entities;

            // Assert

            Assert.Equal(2, queriedRaces.Count());
            Assert.Equivalent(races, queriedRaces);
        }

        [Fact]
        public async Task CanFindObjectById()
        {
            // Arrange

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
            };

            var context = Helpers.CreateInMemoryContext("CanFindRaceById_Db");
            var repository = new RaceDbRepository(context);

            context.Races.AddRange(races);
            await context.SaveChangesAsync();

            // Act

            var foundRace = await repository.FindByIdAsync(2);
            var notFoundRace = await repository.FindByIdAsync(3);

            // Assert

            Assert.NotNull(foundRace);
            Assert.Equivalent(races[1], foundRace);
            Assert.Null(notFoundRace);
        }

        [Fact]
        public async Task CanAddObjects()
        {
            // Arrange

            var raceToAdd = new Race
            {
                Id = 2,
                Name = "Race 2",
                BeginTime = new DateTime(2023, 12, 5),
                IsActive = true,
                IsFreeOrder = false,
                StartAddress = "Sample address"
            };

            var context = Helpers.CreateInMemoryContext("CanAddRace_Db");
            var repository = new RaceDbRepository(context);

            context.Races.Add(new Race
            {
                Id = 1,
                Name = "Race 1",
                BeginTime = new DateTime(2023, 12, 5),
                IsActive = true,
                IsFreeOrder = false,
                StartAddress = "Sample address"
            });

            await context.SaveChangesAsync();

            // Act

            var returnedRace = await repository.AddAsync(raceToAdd);

            // Assert

            Assert.Equal(2, context.Races.Count());
            Assert.Contains(raceToAdd, context.Races);

            Assert.NotNull(returnedRace);
            Assert.Equivalent(raceToAdd, returnedRace);
        }

        [Fact]
        public async Task CannotAddInvalidObject()
        {
            // Arrange

            var raceToAdd = new Race
            {
                Id = 1,
                BeginTime = new DateTime(2023, 12, 5),
                IsActive = true,
                IsFreeOrder = false,
                StartAddress = "Sample address"
            };

            var context = Helpers.CreateInMemoryContext("CannotAddInvalidRace_Db");
            var repository = new RaceDbRepository(context);

            // Assert

            var exception = await Assert.ThrowsAsync<InvalidModelException>(async () =>  await repository.AddAsync(raceToAdd));
            Assert.Single(exception.Errors);
            Assert.Equal(nameof(Race.Name), exception.Errors.Select(e => e.MemberNames.Single()).Single());
        }

        [Fact]
        public async Task CanUpdateObject()
        {
            // Arrange

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
            };

            var context = Helpers.CreateInMemoryContext("CanUpdateRace_Db");
            var repository = new RaceDbRepository(context);

            context.Races.AddRange(races);
            await context.SaveChangesAsync();

            // Act

            await repository.UpdateAsync(2, new Race
            {
                Id = 2,
                Name = "Updated Race 2",
                BeginTime = new DateTime(2023, 12, 5),
                IsActive = true,
                IsFreeOrder = false,
                StartAddress = "Sample address"
            });

            // Assert

            Assert.Equal(2, context.Races.Count());
            Assert.Equal("Updated Race 2", context.Races.Single(r => r.Id == 2).Name);
            Assert.Equal(races[1].Id, context.Races.Single(r => r.Id == 2).Id);
            Assert.Equal(races[1].Description, context.Races.Single(r => r.Id == 2).Description);
            Assert.Equal(races[1].StartAddress, context.Races.Single(r => r.Id == 2).StartAddress);
            Assert.Equal(races[1].BeginTime, context.Races.Single(r => r.Id == 2).BeginTime);
            Assert.Equal(races[1].IsActive, context.Races.Single(r => r.Id == 2).IsActive);
            Assert.Equal(races[1].IsFreeOrder, context.Races.Single(r => r.Id == 2).IsFreeOrder);

            await Assert.ThrowsAsync<InvalidModelException>(async () => await repository.UpdateAsync(2, new Race()));
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await repository.UpdateAsync(10, new Race()));
        }

        [Fact]
        public async Task CanDeleteObject()
        {
            // Arrange

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    BeginTime = new DateTime(2023, 12, 5),
                    IsActive = true,
                    IsFreeOrder = false,
                    StartAddress = "Sample address"
                },
            };

            var context = Helpers.CreateInMemoryContext("CanDeleteRaces_Db");
            var repository = new RaceDbRepository(context);

            context.Races.AddRange(races);
            await context.SaveChangesAsync();

            // Act

            await repository.DeleteAsync(1);

            // Assert

            Assert.Single(context.Races);
            Assert.Equal(2, context.Races.Single().Id);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.DeleteAsync(10));
        }
    }
}
