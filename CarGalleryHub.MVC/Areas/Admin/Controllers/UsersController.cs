using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
