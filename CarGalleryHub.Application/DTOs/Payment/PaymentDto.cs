using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Payment
{
    public class PaymentDto : BaseDateEntityDto
    {
        // ----- //
        public required decimal Amount { get; init; }
        public required PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? FailureReason { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? CardLastFour { get; set; }
        // ----- //
        public required int OrderId { get; init; }

        // ----- //
        public OrderDto Order { get; init; } = null!;
    }
}
