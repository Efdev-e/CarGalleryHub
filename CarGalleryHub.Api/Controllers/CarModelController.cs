using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    public class CarModelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
