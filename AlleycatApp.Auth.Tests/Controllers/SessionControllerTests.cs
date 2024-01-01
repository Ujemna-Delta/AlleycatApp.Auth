using AlleycatApp.Auth.Controllers.Api.User;
using AlleycatApp.Auth.Models.Dto.User;
using AlleycatApp.Auth.Services.Authentication;
using AlleycatApp.Auth.Services.Authentication.Jwt;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AlleycatApp.Auth.Tests.Controllers
{
    public class SessionControllerTests
    {
        [Fact]
        public async Task ReturnsTokenForValidCredentials()
        {
            // Arrange

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(a => a.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new JwtSignInResult { Token = "token" });

            var controller = new SessionController(authServiceMock.Object);

            // Act

            var result = await controller.SignIn(new UserCredentialsDto()) as CreatedAtActionResult;

            // Assert

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("token", result.Value);
        }

        [Fact]
        public async Task ReturnsBadRequestForInvalidCredentials()
        {
            // Arrange

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock.Setup(a => a.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Microsoft.AspNetCore.Identity.SignInResult());

            var controller = new SessionController(authServiceMock.Object);

            // Act

            var result = await controller.SignIn(new UserCredentialsDto()) as BadRequestObjectResult;

            // Assert

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equivalent(new Microsoft.AspNetCore.Identity.SignInResult(), result.Value);
        }
    }
}
