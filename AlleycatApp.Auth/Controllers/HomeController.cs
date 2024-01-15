using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers
{
    public class HomeController(IUserDataProvider userDataProvider) : Controller
    {
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        public async Task<IActionResult> Test()
        {
            var result = "Hello world!";

            if (User.Identity is { IsAuthenticated: true })
                result = $"User signed in as {string.Join(", ", await userDataProvider.GetRolesForClaimsPrincipalAsync(User))}";

            return Ok(result);
        }
    }
}
