using AlleycatApp.Auth.Data;
using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Tests
{
    internal static class Helpers
    {
        public static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(dbName).Options;
            return new ApplicationDbContext(options);
        }

        public static Mock<IRaceRepository> CreateRaceRepositoryMock(IList<Race> races)
        {
            var mock = new Mock<IRaceRepository>();

            mock.SetupGet(r => r.Entities).Returns(races.AsQueryable());
            mock.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => races.SingleOrDefault(race => race.Id == id));
            mock.Setup(r => r.AddAsync(It.IsAny<Race>())).ReturnsAsync((Race race) => race).Callback((Race race) =>
            {
                var context = new ValidationContext(race);
                if (!Validator.TryValidateObject(race, context, null, true)) 
                    throw new InvalidModelException("", null!);

                races.Add(race);
            });
            mock.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Race>())).Callback((int id, Race race) =>
            {
                var raceToUpdate = races.Single(r => r.Id == id);
                var context = new ValidationContext(race);
                if (!Validator.TryValidateObject(race, context, null, true))
                    throw new InvalidModelException("", null!);

                race.CopyTo(raceToUpdate);

            });
            mock.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                .Callback((int id) => races.Remove(races.Single(r => r.Id == id)));

            return mock;
        }

        public static Mock<UserManager<TUser>> GetUserManagerMock<TUser>(IList<TUser> users) where TUser : IdentityUser
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => users.Add(x));

            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new Func<string, TUser?>(userName => users.SingleOrDefault(u => u.UserName == userName)));

            return mgr;
        }
    }
}
