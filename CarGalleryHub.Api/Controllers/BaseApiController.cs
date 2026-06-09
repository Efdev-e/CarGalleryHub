using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int GetUserId() 
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out int userId) ? userId : 0;
        }

        protected bool IsAdmin() => User.IsInRole("Admin");

        protected IActionResult Ok<T>(T data, string message = "işlem başarılı") 
            => base.Ok(new { _data = data, Msg = message });

        protected IActionResult Created<T>(T data, string message = "Oluşturma İşlemi Başarılı")
            => base.StatusCode(201, new { _data = data, Msg = message});

        protected IActionResult Invalid<T>(T data, string message = "İşlem Başarısız")
            => base.StatusCode(401, new { _data = data, Msg = message });
    }
}
