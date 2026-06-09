using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost("Test")]
        [Authorize]
        public IActionResult Pluh([FromBody] string pluh) 
        {
            return Ok("pluh");
        }
    }
}
