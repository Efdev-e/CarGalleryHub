using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Payment
{
    public class PaymentInfoDto : BaseDateEntityDto
    {
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? FailureReason { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? CardLastFour { get; set; }
    }
}
