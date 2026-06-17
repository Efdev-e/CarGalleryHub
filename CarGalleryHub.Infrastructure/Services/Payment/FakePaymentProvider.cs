using CarGalleryHub.Application.Interfaces.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Infrastructure.Services.Payment
{
    public class FakePaymentProvider : IPaymentProvider
    {
        private const string BlockedCard = "1111111111111111";

        public Task<PaymentProviderResult> ChargeAsync(PaymentProviderRequest request)
            => Task.FromResult(Validate(request));

        private static PaymentProviderResult Validate(PaymentProviderRequest request)
        {
            if (request.Amount <= 0)
                return Fail("Ödeme tutarı 0'dan büyük olmalıdır.");
            if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length < 16)
                return Fail("Kart numarası 16 haneli olmalıdır.");
            if (string.IsNullOrWhiteSpace(request.CVV) || request.CVV.Length < 3)
                return Fail("CVV 3 haneli olmalıdır.");
            if (string.IsNullOrWhiteSpace(request.CardHolderName) || request.CardHolderName.Length <= 2)
                return Fail("Kart üzerindeki isim en az 3 karakter olmalıdır.");
            if (request.CardNumber == BlockedCard)
                return Fail("Bu kart bloke edilmiştir, işlem yapılamaz.");

            var transactionId = $"TXN-{Guid.NewGuid():N}".ToUpper()[..20];
            return new PaymentProviderResult(true, transactionId, null);
        }

        private static PaymentProviderResult Fail(string reason)
            => new(false, null, reason);
    }
}
