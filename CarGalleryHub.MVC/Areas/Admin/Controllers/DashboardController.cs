using System.Threading.Tasks;
using CarGalleryHub.Application.DTOs.Dashboard;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApiClient _apiclient;

        public DashboardController(ApiClient apiclient)
        {
            _apiclient = apiclient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiclient.GetAsync<AdminDashboardStatsDto>("api/User/admin/stats");
            if (response != null && response.Success && response.Data != null)
            {
                return View(response.Data);
            }

            return View(new AdminDashboardStatsDto());
        }
    }
}
