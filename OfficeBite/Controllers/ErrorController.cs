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
        [Route("/error/500")]
        public IActionResult Error500()
        {
            return View("InternalServerError");
        }

        [Route("/error/503")]
        public IActionResult Error503()
        {
            return View("ServiceUnavailable");
        }
        public Task<IActionResult> TestError500()
        {

            throw new Exception("Грешка: това е симулирана грешка 500.");
        }
    }
}
