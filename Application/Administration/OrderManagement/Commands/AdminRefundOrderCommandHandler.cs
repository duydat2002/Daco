namespace Daco.Application.Administration.OrderManagement.Commands
{
    public class AdminRefundOrderCommandHandler : IRequestHandler<AdminRefundOrderCommand, ResponseDTO>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminRefundOrderCommandHandler> _logger;

        public AdminRefundOrderCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            ILogger<AdminRefundOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AdminRefundOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} refunding order {OrderId}, reason: {Reason}",
                request.AdminId, request.OrderId, request.Reason);

            var order = await _orderRepository.GetByIdAsync(
                request.OrderId, cancellationToken);

            if (order is null)
                return ResponseDTO.Failure(
                    ErrorCodes.OrderErrors.NotFound,
                    "Order not found");

            if (order.PaymentStatus != PaymentStatus.Paid
                && order.PaymentStatus != PaymentStatus.Refunding)
                return ResponseDTO.Failure(
                    ErrorCodes.OrderErrors.CannotRefund, 
                    $"Cannot refund order with payment status '{order.PaymentStatus}'");

            try
            {
                if (order.PaymentStatus == PaymentStatus.Paid)
                    order.RequestRefund(request.Reason);

                order.CompleteRefund();
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDTO.Failure(
                    ErrorCodes.OrderErrors.CannotRefund,
                    ex.Message);
            }

            _unitOfWork.TrackEntity(order);

            _logger.LogInformation(
                "Order {OrderId} refunded by admin {AdminId}",
                order.Id, request.AdminId);

            return ResponseDTO.Success(new
            {
                order.Id,
                order.OrderCode,
                Status = order.Status.ToString().ToLower(),
                PaymentStatus = order.PaymentStatus.ToString().ToLower(),
            }, "Order refunded successfully");
        }
    }
}
