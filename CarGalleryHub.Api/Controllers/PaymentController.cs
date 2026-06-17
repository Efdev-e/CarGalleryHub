using CarGalleryHub.Application.DTOs.Payment;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Persistence.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService s)
        {
            paymentService = s;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentRequestDto paymentRequestDto)
        {
            var result = await paymentService.ProcessPaymentAsync(GetUserId(), paymentRequestDto);
            if (result.IsSuccess)
                return Ok(result, "Odeme Basarılıdır");
            return BadRequest(new
            {
                success = false,
                message = result.FailureReason,
                data = result
            });
        }
    }
}
