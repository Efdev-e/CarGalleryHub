using CarGalleryHub.Application.Interfaces.Payment;
using Microsoft.Extensions.Options;
using CarGalleryHub.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Infrastructure.Services.Payment
{
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IyzicoOptions _iyzicoOptions;

        public PaymentProviderFactory(IOptions<CarGalleryHub.Infrastructure.Options.IyzicoOptions> iyzicoOptions)
        {
            _iyzicoOptions = iyzicoOptions.Value;
        }

        public IPaymentProvider Create(string providerName = "fake")
        {
            return providerName.ToLower() switch
            {
                "fake" => new FakePaymentProvider(),
                "iyzico" => new IyzicoPaymentProvider(_iyzicoOptions),
                _ => throw new InvalidOperationException($"Bilinmeyen ödeme sağlayıcısı: {providerName}")
            };
        }
    }
}
