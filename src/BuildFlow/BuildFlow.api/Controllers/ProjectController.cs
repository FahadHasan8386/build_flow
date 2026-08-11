using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
