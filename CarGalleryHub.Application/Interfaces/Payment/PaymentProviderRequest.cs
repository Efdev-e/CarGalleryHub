namespace CarGalleryHub.Application.Interfaces.Payment
{
    public record PaymentProviderRequest(
        string CardNumber,
        string CardHolderName,
        string ExpiryYear,
        string ExpiryMonth,
        string CVV,
        decimal Amount
    );
}
