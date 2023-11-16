using AlleycatApp.Auth.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void DoesReturnHelloWorld()
        {
            // Arrange

            var controller = new HomeController();

            // Act

            var result = controller.Index() as OkObjectResult;

            // Assert

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Hello world!", result.Value);
        }
    }
}
