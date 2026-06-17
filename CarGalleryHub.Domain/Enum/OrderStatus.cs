namespace CarGalleryHub.Domain.Enum
{
    public enum OrderStatus
    {
        WaitingPayment,
        Pending,
        Completed,
        Cancelled,
        Refunded,
        Paid,
        PaymentFailed
    }
}
