using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
