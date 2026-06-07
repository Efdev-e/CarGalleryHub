namespace CarGalleryHub.Application.Interfaces.Payment
{
    public record PaymentProviderResult(
        bool IsSuccess,
        string? TransactionId,
        string? FailureReason
    );
}
