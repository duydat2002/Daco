namespace Daco.Domain.Orders.Entities
{
    public class Payment : Entity
    {
        public Guid          OrderId              { get; private set; }
        public PaymentMethod PaymentMethod        { get; private set; }
        public decimal       Amount               { get; private set; }
        public string?       PaymentGateway       { get; private set; }
        public string?       GatewayTransactionId { get; private set; }
        public string?       GatewayResponse      { get; private set; }
        public PaymentStatus Status               { get; private set; }
        public DateTime      CreatedAt            { get; private set; }
        public DateTime?     PaidAt               { get; private set; }
        public DateTime?     FailedAt             { get; private set; }
        public string?       FailureReason        { get; private set; }

        protected Payment () { }

        public static Payment Create(
            Guid orderId,
            PaymentMethod paymentMethod,
            decimal amount,
            string? paymentGateway = null)
        {
            Guard.Against(orderId == Guid.Empty, "OrderId is required");
            Guard.AgainstNegativeOrZero(amount, nameof(amount));

            return new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethod = paymentMethod,
                Amount = amount,
                PaymentGateway = paymentGateway,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MarkAsPaid(string? gatewayTransactionId = null, string? gatewayResponse = null)
        {
            Guard.Against(Status == PaymentStatus.Paid, "Payment is already paid");
            Guard.Against(
                Status != PaymentStatus.Pending,
                "Only pending payments can be marked as paid");

            Status = PaymentStatus.Paid;
            GatewayTransactionId = gatewayTransactionId;
            GatewayResponse = gatewayResponse;
            PaidAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string reason, string? gatewayResponse = null)
        {
            Guard.AgainstNullOrEmpty(reason, nameof(reason));
            Guard.Against(
                Status != PaymentStatus.Pending,
                "Only pending payments can be marked as failed");

            Status = PaymentStatus.Failed;
            FailureReason = reason;
            GatewayResponse = gatewayResponse;
            FailedAt = DateTime.UtcNow;
        }

        public void MarkAsRefunding()
        {
            Guard.Against(Status != PaymentStatus.Paid, "Only paid payments can be refunded");

            Status = PaymentStatus.Refunding;
        }

        public void MarkAsRefunded()
        {
            Guard.Against(Status != PaymentStatus.Refunding, "Payment is not in refunding state");

            Status = PaymentStatus.Refunded;
        }
    }
}
