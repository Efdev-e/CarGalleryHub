using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CarController(IUnitOfWork work)
        {
            unitOfWork = work;
        }


    }
}
