namespace CarGalleryHub.Application.DTOs.Payment
{
    public class PaymentResultDto
    {
        public bool IsSuccess { get; set; }
        public string? TransactionId { get; set; }
        public string? FailureReason { get; set; }
        public decimal Amount { get; set; }
    }
}
