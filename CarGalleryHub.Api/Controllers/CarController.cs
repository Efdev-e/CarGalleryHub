using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
