using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarModelsController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
