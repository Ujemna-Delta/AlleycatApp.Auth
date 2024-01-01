using AlleycatApp.Auth.Services.Providers;
using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers
{
    public class HomeController(IUserDataProvider userDataProvider) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = "Hello world!";

            if (User.Identity is { IsAuthenticated: true })
                result = $"User signed in as {string.Join(", ", await userDataProvider.GetRolesForClaimsPrincipalAsync(User))}";

            return Ok(result);
        }
    }
}
