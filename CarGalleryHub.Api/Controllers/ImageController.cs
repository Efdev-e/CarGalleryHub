using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public ImageController(IUnitOfWork work)
        {
            unitOfWork = work;
        }

        [HttpGet("{*url}")]
        public async Task<IActionResult> GetImage(string url) 
        {
            var image = await unitOfWork.Images.FirstOrDefaultAsync(x => x.ImageUrl == url);
            if (image is null) return Invalid("Yok");

            return Ok(image.ImageData);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateImage(ImageDto imageDto) 
        {
            if (imageDto is null) return Invalid("ImageDto invalid");

            using (var client = new HttpClient()) 
            {
                try 
                {
                    byte[] imagebytes = await client.GetByteArrayAsync(imageDto.ImageUrl);

                    var image = new Image()
                    {
                        ImageUrl = imageDto.ImageUrl,
                        ImageType = imageDto.ImageType,
                        ImageData = imagebytes
                    };

                    await unitOfWork.Images.AddAsync(image);
                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception e) 
                {
                    Console.WriteLine($"Eror: {e.Message}");
                    return Invalid();
                }
            }

            return Ok();
        }

        [HttpDelete("delete/{imgurl}")]
        [Authorize]
        public async Task<IActionResult> deleteImage(string imgurl) 
        {
            var image = await unitOfWork.Images.FirstOrDefaultAsync(x => x.ImageUrl == imgurl);
            if (image is null) return Invalid("Yok");
            unitOfWork.Images.Remove(image);
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
