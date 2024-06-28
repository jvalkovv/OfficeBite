using Microsoft.AspNetCore.Mvc;

namespace OfficeBite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file received");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages", imageFile.FileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return Content("Image uploaded successfully");
        }

        public IActionResult CaptureImage()
        {
            return View();
        }
    }
}
