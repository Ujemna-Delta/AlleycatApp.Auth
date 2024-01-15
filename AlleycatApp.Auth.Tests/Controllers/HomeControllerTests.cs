using System.Security.Claims;
using AlleycatApp.Auth.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task DoesReturnHelloWorld()
        {
            // Arrange

            var controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() } };
            var controller = new HomeController(null!) { ControllerContext = controllerContext };

            // Act

            var result = await controller.Test() as OkObjectResult;

            // Assert

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Hello world!", result.Value);
        }
    }
}
