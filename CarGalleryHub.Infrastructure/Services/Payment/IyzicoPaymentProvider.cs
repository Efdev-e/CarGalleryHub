using CarGalleryHub.Application.Interfaces.Payment;
using CarGalleryHub.Infrastructure.Options;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CarGalleryHub.Infrastructure.Services.Payment
{
    public class IyzicoPaymentProvider : IPaymentProvider
    {

        private readonly Iyzipay.Options _iyzicoOptions;

        public IyzicoPaymentProvider(IyzicoOptions iyzicoOptions)
        {
            _iyzicoOptions = new Iyzipay.Options
            {
                ApiKey = iyzicoOptions.ApiKey,
                SecretKey = iyzicoOptions.SecretKey,
                BaseUrl = iyzicoOptions.BaseUrl,
            };
        }

        public async Task<PaymentProviderResult> ChargeAsync(PaymentProviderRequest paymentProviderRequest)
        {
            // Iyzico Sandbox has a limit of 100,000 TL per transaction.
            // If the amount is >= 100,000 TL, we cap it at 99,000 TL to allow sandbox testing.
            var iyzicoAmount = paymentProviderRequest.Amount >= 100000
                ? 99000.00m
                : paymentProviderRequest.Amount;

            var paymentRequest = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = Guid.NewGuid().ToString(),
                Price = iyzicoAmount.ToString("F2", CultureInfo.InvariantCulture),
                PaidPrice = iyzicoAmount.ToString("F2", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = Guid.NewGuid().ToString(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                PaymentCard = new PaymentCard
                {
                    CardHolderName = paymentProviderRequest.CardHolderName,
                    CardNumber = paymentProviderRequest.CardNumber,
                    ExpireMonth = paymentProviderRequest.ExpiryMonth,
                    ExpireYear = paymentProviderRequest.ExpiryYear,
                    Cvc = paymentProviderRequest.CVV,
                    RegisterCard = 0
                },
                Buyer = new Buyer
                {
                    Id = "B001",
                    Name = GetBuyerName(paymentProviderRequest.CardHolderName),
                    Surname = GetBuyerSurname(paymentProviderRequest.CardHolderName),
                    Email = "john.doe@example.com",
                    IdentityNumber = "12345678911",
                    RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                    City = "Ýstanbul",
                    Country = "Turkiye",
                    ZipCode = "34732",
                    Ip = "127.0.0.1"
                },
                ShippingAddress = new Address
                {
                    ContactName = paymentProviderRequest.CardHolderName,
                    City = "Ýstanbul",
                    Country = "Turkiye",
                    Description = "TEST ADRES"

                },
                BillingAddress = new Address
                {
                    ContactName = paymentProviderRequest.CardHolderName,
                    City = "Ýstanbul",
                    Country = "Turkiye",
                    Description = "TEST ADRES"
                },

                BasketItems = new List<BasketItem>
                 {
                     new BasketItem
                     {
                         Id="B001",
                         Name="Sipariþ Ödemesi ",
                         Category1="Genel",
                         ItemType=BasketItemType.PHYSICAL.ToString(),
                         Price=iyzicoAmount.ToString("F2", CultureInfo.InvariantCulture)
                     }
                 }



            };


            var payment = await Task.Run(() =>
               Iyzipay.Model.Payment.Create(paymentRequest, _iyzicoOptions));

            if (payment.Status == "success")
            {
                return new PaymentProviderResult(true, payment.PaymentId, null);
            }
            return new PaymentProviderResult(false, null, payment.ErrorMessage);
        }
        private static string GetBuyerName(string cardHolderName)
        {
            var parts = cardHolderName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 0 ? "Musteri" : parts[0];
        }

        private static string GetBuyerSurname(string cardHolderName)
        {
            var parts = cardHolderName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length <= 1 ? "CommerceHub" : parts[^1];
        }
    }
}
