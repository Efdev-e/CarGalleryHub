using CarGalleryHub.Application.DTOs.Payment;
using CarGalleryHub.Application.Exceptions;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Application.Interfaces.Payment;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentProviderFactory _paymentProviderFactory;
        private readonly string _defaultProvider;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IPaymentProviderFactory paymentProviderFactory,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _paymentProviderFactory = paymentProviderFactory;
            _defaultProvider = configuration["PaymentProvider"] ?? "fake";
        }


        public async Task<PaymentResultDto> ProcessPaymentAsync(int userId, PaymentRequestDto dto)
        {
            var order = await _unitOfWork.Orders.Query()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Id == dto.OrderId && x.UserId == userId)
                ?? throw new NotFound($"Sipariş bulunamadı ID: {dto.OrderId}");
            Console.WriteLine(order.Payment);
            if (order.OrderStatus == OrderStatus.Paid || order.OrderStatus == OrderStatus.Completed)
                throw new AppException("Already Paid", 500);
            if (order.Payment != null && order.Payment.Status == PaymentStatus.Success)
                throw new AppException("Bu sipariş için zaten ödeme yapılmıstır..",500);

            var provider = _paymentProviderFactory.Create(_defaultProvider);

            var providerResult = await provider.ChargeAsync(new PaymentProviderRequest(
                dto.CardNumber,
                dto.CardHolderNumner,
                dto.ExpiryYear,
                dto.ExpiryMonth,
                dto.Cvv,
                order.TotalCost
                ));

            if (order.Payment != null)
            {
                order.Payment.Status = providerResult.IsSuccess ? PaymentStatus.Success : PaymentStatus.Failed;
                order.Payment.TransactionId = providerResult.TransactionId;
                order.Payment.FailureReason = providerResult.FailureReason;
                order.Payment.CardLastFour = dto.CardNumber.Length >= 4 ? dto.CardNumber[^4..] : dto.CardNumber;
                order.Payment.PaidAt = providerResult.IsSuccess ? DateTime.UtcNow : (DateTime?)null;
                _unitOfWork.Payments.Update(order.Payment);
            }
            else
            {
                var payment = new CarGalleryHub.Domain.Entities.Payment
                {
                    OrderId = order.Id,
                    Amount = order.TotalCost,
                    Status = providerResult.IsSuccess ? PaymentStatus.Success : PaymentStatus.Failed,
                    TransactionId = providerResult.TransactionId,
                    FailureReason = providerResult.FailureReason,
                    CardLastFour = dto.CardNumber.Length >= 4 ? dto.CardNumber[^4..] : dto.CardNumber,
                    PaidAt = providerResult.IsSuccess ? DateTime.UtcNow : (DateTime?)null
                };
                await _unitOfWork.Payments.AddAsync(payment);
            }

            order.OrderStatus = providerResult.IsSuccess ? OrderStatus.Paid : OrderStatus.PaymentFailed;
            order.Updated();
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();


            return new PaymentResultDto
            {
                IsSuccess = providerResult.IsSuccess,
                TransactionId = providerResult.TransactionId,
                FailureReason = providerResult.FailureReason,
                Amount = order.TotalCost

            };


        }
    }
}
