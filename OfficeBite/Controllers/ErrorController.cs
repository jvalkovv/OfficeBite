using Microsoft.AspNetCore.Mvc;

namespace OfficeBite.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/error/404")]
        public IActionResult Error404()
        {
            return View("NotFound");
        }
    }
}
