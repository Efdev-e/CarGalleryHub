namespace CarGalleryHub.Application.Interfaces.Payment
{
    public interface IPaymentProvider 
    {
        Task<PaymentProviderResult> ChargeAsync(PaymentProviderRequest paymentProviderRequest);
    }
}
