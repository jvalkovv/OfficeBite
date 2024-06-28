
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

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult UploadImage(string imageData)
        {
            if (string.IsNullOrEmpty(imageData))
            {
                return BadRequest("No image data received");
            }

            var imageBytes = Convert.FromBase64String(imageData.Split(',')[1]);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages", "image.png");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Ensure directory exists
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            return Content("Image uploaded successfully");
        }

        public IActionResult CaptureImage()
        {
            return View();
        }
    }
}
