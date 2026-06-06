using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Payment : BaseDateEntity
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
        public Order Order { get; init; } = null!;
    }
}
