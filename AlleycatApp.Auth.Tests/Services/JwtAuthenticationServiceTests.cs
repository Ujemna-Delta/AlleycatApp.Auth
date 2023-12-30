using System.Security.Claims;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AlleycatApp.Auth.Tests.Services
{
    public class JwtAuthenticationServiceTests
    {
        [Fact]
        public async Task CanSignInUsingValidCredentials()
        {
            // Arrange

            var users = new List<IdentityUser>
            {
                new() { UserName = "user1" }
            };

            var userManagerMock = Helpers.GetUserManagerMock(users);
            userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync((IdentityUser _, string password) => password == "validPassword");

            var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            tokenGeneratorMock.Setup(g => g.GenerateToken(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var service = new JwtAuthenticationService(userManagerMock.Object, tokenGeneratorMock.Object);

            // Act

            var result = await service.SignInAsync("user1", "validPassword");

            // Assert

            Assert.NotNull(result);
            Assert.IsType<JwtSignInResult>(result);
            Assert.Equal("token", (result as JwtSignInResult)?.Token);
        }

        [Fact]
        public async Task CannotSignInUsingInvalidPassword()
        {
            // Arrange

            var users = new List<IdentityUser>
            {
                new() { UserName = "user1" }
            };

            var userManagerMock = Helpers.GetUserManagerMock(users);
            userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync((IdentityUser _, string password) => password == "validPassword");

            var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();

            var service = new JwtAuthenticationService(userManagerMock.Object, tokenGeneratorMock.Object);

            // Act

            var result = await service.SignInAsync("user1", "invalidPassword");

            // Assert

            Assert.NotNull(result);
            Assert.IsNotType<JwtSignInResult>(result);
            Assert.False(result.Succeeded);
            tokenGeneratorMock.Verify(g => g.GenerateToken(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Fact]
        public async Task CannotSignInNonExistingUser()
        {
            // Arrange

            var users = new List<IdentityUser>();
            var userManagerMock = Helpers.GetUserManagerMock(users);
            var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            var service = new JwtAuthenticationService(userManagerMock.Object, tokenGeneratorMock.Object);

            // Act

            var result = await service.SignInAsync("user1", "password");

            // Assert

            Assert.NotNull(result);
            Assert.IsNotType<JwtSignInResult>(result);
            Assert.False(result.Succeeded);
            tokenGeneratorMock.Verify(g => g.GenerateToken(It.IsAny<IEnumerable<Claim>>()), Times.Never);
            userManagerMock.Verify(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        }
    }
}
