using AlleycatApp.Auth.Controllers.Api;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AlleycatApp.Auth.Tests.Controllers
{
    public class RacesControllerTests
    {
        [Fact]
        public void CanGetAllRaces()
        {
            // Arrange

            var raceDto = new RaceDto
            {
                Id = 1,
                Name = "Race 1",
                StartAddress = "Sample address"
            };

            var races = new List<Race> 
            { 
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    StartAddress = "Sample address"
                }
            };

            var repositoryMock = Helpers.CreateRaceRepositoryMock(races);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<RaceDto>(races[0])).Returns(raceDto);

            var controller = new RacesController(repositoryMock.Object, null!, mapperMock.Object);

            // Act

            var result = controller.GetRaces() as OkObjectResult;
            var dtos = result?.Value as IEnumerable<RaceDto>;

            // Assert

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(dtos);
            Assert.Equal(raceDto, dtos.Single());
        }

        [Fact]
        public async Task CanGetRaceById()
        {
            // Arrange

            var raceDto = new RaceDto
            {
                Id = 2,
                Name = "Race 2",
                StartAddress = "Sample address"
            };

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    StartAddress = "Sample address"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    StartAddress = "Sample address"
                }
            };

            var repositoryMock = Helpers.CreateRaceRepositoryMock(races);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<RaceDto>(races[1])).Returns(raceDto);

            var controller = new RacesController(repositoryMock.Object, null!, mapperMock.Object);

            // Act

            var result1 = await controller.GetRaceById(2) as OkObjectResult;
            var result2 = await controller.GetRaceById(10) as NotFoundResult;

            // Assert

            Assert.NotNull(result1);
            Assert.NotNull(result2);

            Assert.Equal(200, result1.StatusCode);
            Assert.Equal(404, result2.StatusCode);

            Assert.Equal(raceDto, result1.Value);
        }

        [Fact]
        public async Task CanAddRace()
        {
            // Arrange

            var raceDto = new RaceDto
            {
                Id = 2,
                Name = "Race 2",
                StartAddress = "Sample address"
            };

            var expectedRace = new Race
            {
                Id = 2,
                Name = "Race 2",
                StartAddress = "Sample address"
            };

            var invalidRaceDto = new RaceDto { Id = 2 };
            var invalidRace = new Race { Id = 2 };

            var races = new List<Race>();

            var repositoryMock = Helpers.CreateRaceRepositoryMock(races);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<RaceDto>(expectedRace)).Returns(raceDto);
            mapperMock.Setup(m => m.Map<Race>(raceDto)).Returns(expectedRace);
            mapperMock.Setup(m => m.Map<Race>(invalidRaceDto)).Returns(invalidRace);

            var controller = new RacesController(repositoryMock.Object, null!, mapperMock.Object);

            // Act

            var result1 = await controller.AddRace(raceDto) as CreatedAtActionResult;
            var result2 = await controller.AddRace(invalidRaceDto) as BadRequestObjectResult;

            // Assert

            Assert.NotNull(result1);
            Assert.NotNull(result2);

            Assert.Equal(201, result1.StatusCode);
            Assert.Equal(400, result2.StatusCode);

            Assert.Equivalent(raceDto, result1.Value);
        }

        [Fact]
        public async Task CanUpdateRace()
        {
            // Arrange

            var raceDto = new RaceDto
            {
                Id = 20,
                Name = "Updated name",
                StartAddress = "Updated address"
            };

            var race = new Race
            {
                Id = 20,
                Name = "Updated name",
                StartAddress = "Updated address"
            };

            var invalidRaceDto = new RaceDto { Id = 2 };
            var invalidRace = new Race { Id = 2 };

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    StartAddress = "Sample address 1"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    StartAddress = "Sample address 2"
                },
            };

            var repositoryMock = Helpers.CreateRaceRepositoryMock(races);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Race>(raceDto)).Returns(race);
            mapperMock.Setup(m => m.Map<Race>(invalidRaceDto)).Returns(invalidRace);

            var controller = new RacesController(repositoryMock.Object, null!, mapperMock.Object);

            // Act

            var result1 = await controller.UpdateRace(2, raceDto) as NoContentResult;
            var result2 = await controller.UpdateRace(1, invalidRaceDto) as BadRequestObjectResult;
            var result3 = await controller.UpdateRace(20, raceDto) as NotFoundResult;

            // Assert

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);

            Assert.Equal(204, result1.StatusCode);
            Assert.Equal(400, result2.StatusCode);
            Assert.Equal(404, result3.StatusCode);

            Assert.Equal(race.Name, races[1].Name);
            Assert.Equal(race.StartAddress, races[1].StartAddress);
            Assert.Equal(2, races[1].Id);
        }

        [Fact]
        public async Task CanDeleteRace()
        {
            // Arrange

            var races = new List<Race>
            {
                new()
                {
                    Id = 1,
                    Name = "Race 1",
                    StartAddress = "Sample address 1"
                },
                new()
                {
                    Id = 2,
                    Name = "Race 2",
                    StartAddress = "Sample address 2"
                },
            };

            var repositoryMock = Helpers.CreateRaceRepositoryMock(races);
            var mapperMock = new Mock<IMapper>();
            var controller = new RacesController(repositoryMock.Object, null!, mapperMock.Object);

            // Act

            var result1 = await controller.DeleteRace(1) as NoContentResult;
            var result2 = await controller.DeleteRace(30) as NotFoundResult;

            // Assert

            Assert.NotNull(result1);
            Assert.NotNull(result2);

            Assert.Equal(204, result1.StatusCode);
            Assert.Equal(404, result2.StatusCode);

            Assert.Single(races);
            Assert.Equal(2, races.Single().Id);
        }
    }
}
