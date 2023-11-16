using Microsoft.AspNetCore.Mvc;

namespace AlleycatApp.Auth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello world!");
        }
    }
}
