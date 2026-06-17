using CarGalleryHub.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(int userId, PaymentRequestDto dto);
    }
}
