using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
