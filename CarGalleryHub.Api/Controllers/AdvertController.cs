using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    public class AdvertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
