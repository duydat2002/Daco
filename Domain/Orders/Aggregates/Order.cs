namespace Daco.Domain.Orders.Aggregates
{
    public class Order : AggregateRoot
    {
        private readonly List<OrderItem> _orderItems = new();
        private readonly List<OrderStatusHistory> _statusHistory = new();
        private readonly List<Payment> _payments = new();

        // --- Identity ---
        public Guid            UserId           { get; private set; }
        public Guid            ShopId           { get; private set; }
        public string          OrderCode        { get; private set; }
        // --- Status         
        public OrderStatus     Status           { get; private set; }
        public PaymentStatus   PaymentStatus    { get; private set; }
        // --- Payment         
        public PaymentMethod   PaymentMethod    { get; private set; }
        // ---Pricing         
        public decimal         Subtotal         { get; private set; }  // Tổng tiền hàng
        public decimal         ShippingFee      { get; private set; }
        public decimal         DiscountAmount   { get; private set; }  // Từ voucher
        public decimal         TotalAmount      { get; private set; }  // Khách thực trả
        // --- Shipping address snapshot ---
        public ShippingAddress ShippingAddress  { get; private set; }
        public string          RecipientName    { get; private set; }
        public string          RecipientPhone   { get; private set; }
        // --- Notes           ---
        public string?         BuyerNote        { get; private set; }
        public string?         SellerNote       { get; private set; }
        // --- Cancellation    ---
        public string?         CancelledBy      { get; private set; }
        public string?         CancelReason     { get; private set; }
        public DateTime?       CancelledAt      { get; private set; }
        // --- Timestamps      ---
        public DateTime        CreatedAt        { get; private set; }
        public DateTime?       UpdatedAt        { get; private set; }
        public DateTime?       ConfirmedAt      { get; private set; }
        public DateTime?       ShippedAt        { get; private set; }
        public DateTime?       DeliveredAt      { get; private set; }
        public DateTime?       CompletedAt      { get; private set; }

        // --- Collections ---
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public IReadOnlyCollection<OrderStatusHistory> StatusHistory => _statusHistory.AsReadOnly();
        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
        protected Order() { }

        
        public static Order Create(
            Guid userId,
            Guid shopId,
            string orderCode,
            PaymentMethod paymentMethod,
            ShippingAddress shippingAddress,
            string recipientName,
            string recipientPhone,
            decimal shippingFee = 0m,
            decimal discountAmount = 0m,
            string? buyerNote = null)
        {
            Guard.Against(userId == Guid.Empty, "UserId is required");
            Guard.Against(shopId == Guid.Empty, "ShopId is required");
            Guard.AgainstNullOrEmpty(orderCode, nameof(orderCode));
            Guard.AgainstNullOrEmpty(recipientName, nameof(recipientName));
            Guard.AgainstNullOrEmpty(recipientPhone, nameof(recipientPhone));
            Guard.AgainstNull(shippingAddress, nameof(shippingAddress));
            Guard.AgainstNegative(shippingFee, nameof(shippingFee));
            Guard.AgainstNegative(discountAmount, nameof(discountAmount));

            var isPrepaid = paymentMethod != PaymentMethod.Cod;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ShopId = shopId,
                OrderCode = orderCode,
                PaymentMethod = paymentMethod,
                Status = isPrepaid ? OrderStatus.PendingPayment : OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Unpaid,
                ShippingAddress = shippingAddress,
                RecipientName = recipientName,
                RecipientPhone = recipientPhone,
                ShippingFee = shippingFee,
                DiscountAmount = discountAmount,
                Subtotal = 0m,
                TotalAmount = 0m,
                BuyerNote = buyerNote,
                CreatedAt = DateTime.UtcNow
            };

            order.RecordHistory(order.Status, note: "Order placed");

            return order;
        }

        // Item
        public void AddItem(
            Guid productId,
            Guid? variantId,
            string productName,
            string? variantName,
            string? productImage,
            string? sku,
            decimal price,
            int quantity,
            decimal discountAmount = 0m)
        {
            Guard.Against(
                Status != OrderStatus.PendingPayment && Status != OrderStatus.Pending,
                "Cannot add items to an order that is already being processed");

            var item = OrderItem.Create(
                Id, productId, variantId,
                productName, variantName, productImage, sku,
                price, quantity, discountAmount);

            _orderItems.Add(item);
            RecalculateTotals();
            UpdatedAt = DateTime.UtcNow;
        }


        // Payment
        public Payment AddPayment(string? paymentGateway = null)
        {
            Guard.Against(
                Status != OrderStatus.PendingPayment,
                "Cannot add payment to this order status");

            Guard.Against(
                _payments.Any(p => p.Status == PaymentStatus.Pending),
                "There is already a pending payment");

            var payment = Payment.Create(Id, PaymentMethod, TotalAmount, paymentGateway);
            _payments.Add(payment);

            return payment;
        }

        public void MarkAsPaid()
        {
            Guard.Against(
                PaymentStatus == PaymentStatus.Paid,
                "Order is already paid");

            Guard.Against(
                Status != OrderStatus.PendingPayment && Status != OrderStatus.Pending,
                "Cannot mark payment for this order status");

            PaymentStatus = PaymentStatus.Paid;
            Status = OrderStatus.Pending;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, note: "Payment received");

            AddDomainEvent(new OrderPaidEvent(Id, UserId, ShopId, TotalAmount));
        }

        public void MarkPaymentFailed(string reason)
        {
            Guard.AgainstNullOrEmpty(reason, nameof(reason));
            Guard.Against(Status != OrderStatus.PendingPayment, "Order is not awaiting payment");

            PaymentStatus = PaymentStatus.Failed;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, note: $"Payment failed: {reason}");
        }

        // Status transitions
        public void Confirm(Guid confirmedBy)
        {
            Guard.Against(Status != OrderStatus.Pending, "Only pending orders can be confirmed");
            Guard.Against(PaymentMethod != PaymentMethod.Cod && PaymentStatus != PaymentStatus.Paid,
                "Prepaid order must be paid before confirming");

            Status = OrderStatus.Confirmed;
            ConfirmedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, createdBy: confirmedBy, note: "Order confirmed by seller");

            AddDomainEvent(new OrderConfirmedEvent(Id, ShopId, UserId));
        }

        public void StartProcessing(Guid processedBy)
        {
            Guard.Against(Status != OrderStatus.Confirmed, "Only confirmed orders can be processed");

            Status = OrderStatus.Processing;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, createdBy: processedBy, note: "Preparing shipment");
        }

        public void Ship(Guid shippedBy, string trackingNumber)
        {
            Guard.Against(Status != OrderStatus.Processing, "Only processing orders can be shipped");
            Guard.AgainstNullOrEmpty(trackingNumber, nameof(trackingNumber));

            Status = OrderStatus.Shipping;
            ShippedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, createdBy: shippedBy, note: $"Tracking: {trackingNumber}");

            AddDomainEvent(new OrderShippedEvent(Id, UserId, ShopId, trackingNumber));
        }

        public void MarkAsDelivered()
        {
            Guard.Against(Status != OrderStatus.Shipping, "Only shipping orders can be marked as delivered");

            Status = OrderStatus.Delivered;
            DeliveredAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            if (PaymentMethod == PaymentMethod.Cod)
                PaymentStatus = PaymentStatus.Paid;

            RecordHistory(Status, note: "Delivered successfully");

            AddDomainEvent(new OrderDeliveredEvent(Id, UserId, ShopId));
        }

        public void Complete()
        {
            Guard.Against(Status != OrderStatus.Delivered, "Only delivered orders can be completed");

            Status = OrderStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, note: "Order completed");

            AddDomainEvent(new OrderCompletedEvent(Id, UserId, ShopId, TotalAmount));
        }

        // Cancellation
        public void Cancel(string cancelledBy, string reason, Guid? cancelledById = null)
        {
            Guard.AgainstNullOrEmpty(cancelledBy, nameof(cancelledBy));
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            var cancellableStatuses = new[]
            {
                OrderStatus.PendingPayment,
                OrderStatus.Pending,
                OrderStatus.Confirmed
            };

            Guard.Against(
                !cancellableStatuses.Contains(Status),
                $"Order cannot be cancelled at status {Status}");

            var wasPaid = PaymentStatus == PaymentStatus.Paid;

            Status = OrderStatus.Cancelled;
            CancelledBy = cancelledBy;
            CancelReason = reason;
            CancelledAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            if (wasPaid)
                PaymentStatus = PaymentStatus.Refunding;

            RecordHistory(Status, createdBy: cancelledById, note: $"Cancelled by {cancelledBy}: {reason}");

            AddDomainEvent(new OrderCancelledEvent(Id, UserId, ShopId, cancelledBy, reason, wasPaid));
        }

        // Refund
        public void RequestRefund(string reason)
        {
            Guard.AgainstNullOrEmpty(reason, nameof(reason));
            Guard.Against(
                Status != OrderStatus.Delivered && Status != OrderStatus.Completed,
                "Refund can only be requested for delivered or completed orders");

            Guard.Against(PaymentStatus != PaymentStatus.Paid, "Order has not been paid");

            Status = OrderStatus.Refunding;
            PaymentStatus = PaymentStatus.Refunding;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, note: $"Refund requested: {reason}");

            AddDomainEvent(new OrderRefundRequestedEvent(Id, UserId, ShopId, TotalAmount));
        }

        public void CompleteRefund()
        {
            Guard.Against(Status != OrderStatus.Refunding, "Order is not in refunding state");

            Status = OrderStatus.Refunded;
            PaymentStatus = PaymentStatus.Refunded;
            UpdatedAt = DateTime.UtcNow;

            RecordHistory(Status, note: "Refund completed");

            AddDomainEvent(new OrderRefundCompletedEvent(Id, UserId, ShopId, TotalAmount));
        }

        // Notes
        public void UpdateSellerNote(string note)
        {
            SellerNote = note;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Publish()
        {
            AddDomainEvent(new OrderPlacedEvent(Id, UserId, ShopId, OrderCode, TotalAmount, PaymentMethod));
        }

        public bool CanBeCancelledByBuyer()
            => Status == OrderStatus.PendingPayment || Status == OrderStatus.Pending;

        public bool CanBeCancelledBySeller()
            => Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;

        public bool IsCancellable()
            => CanBeCancelledByBuyer() || Status == OrderStatus.Confirmed;

        private void RecalculateTotals()
        {
            Subtotal = _orderItems.Sum(i => i.TotalAmount);
            TotalAmount = Subtotal + ShippingFee - DiscountAmount;

            Guard.Against(TotalAmount < 0, "Total amount cannot be negative");
        }

        private void RecordHistory(OrderStatus status, Guid? createdBy = null, string? note = null)
        {
            _statusHistory.Add(OrderStatusHistory.Create(Id, status, createdBy, note));
        }
    }
}
