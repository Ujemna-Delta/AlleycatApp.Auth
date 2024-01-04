using AlleycatApp.Auth.Infrastructure.Exceptions;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Repositories;
using AlleycatApp.Auth.Repositories.Races;
using AutoMapper;
using Moq;

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
            var repository = new RaceDbRepository(context, null!);

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
            var repository = new RaceDbRepository(context, null!);

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
            var repository = new RaceDbRepository(context, null!);

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
            var repository = new RaceDbRepository(context, null!);

            // Assert

            var exception = await Assert.ThrowsAsync<InvalidModelException>(async () =>  await repository.AddAsync(raceToAdd));
            Assert.Single(exception.Errors!);
            Assert.Equal(nameof(Race.Name), exception.Errors?.Select(e => e.MemberNames.Single()).Single());
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

            var updatedRace = new Race
            {
                Id = 2,
                Name = "Updated Race 2",
                BeginTime = new DateTime(2023, 12, 5),
                IsActive = true,
                IsFreeOrder = false,
                StartAddress = "Sample address"
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map(It.IsAny<Race>(), It.IsAny<Race>())).Returns(updatedRace)
                .Callback((Race _, Race _) =>
                {
                    races[1].Name = updatedRace.Name;
                    races[1].BeginTime = updatedRace.BeginTime;
                    races[1].IsActive = updatedRace.IsActive;
                    races[1].Description = updatedRace.Description;
                    races[1].IsFreeOrder = updatedRace.IsFreeOrder;
                    races[1].StartAddress = updatedRace.StartAddress;
                    races[1].ValueModifier = updatedRace.ValueModifier;
                });

            var context = Helpers.CreateInMemoryContext("CanUpdateRace_Db");
            var repository = new RaceDbRepository(context, mapperMock.Object);

            context.Races.AddRange(races);
            await context.SaveChangesAsync();

            // Act

            await repository.UpdateAsync(2, updatedRace);

            // Assert

            Assert.Equal(2, context.Races.Count());
            Assert.Equivalent(races[1], updatedRace);

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
            var repository = new RaceDbRepository(context, null!);

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
