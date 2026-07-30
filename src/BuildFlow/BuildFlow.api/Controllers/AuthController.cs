using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
