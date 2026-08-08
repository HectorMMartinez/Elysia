using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    public class CenterIAController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
