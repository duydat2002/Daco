namespace Daco.Application.Administration.OrderManagement.Commands
{
    public class AdminCancelOrderCommandHandler : IRequestHandler<AdminCancelOrderCommand, ResponseDTO>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminCancelOrderCommandHandler> _logger;

        public AdminCancelOrderCommandHandler(
            IOrderRepository orderRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<AdminCancelOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AdminCancelOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} cancelling order {OrderId}, reason: {Reason}",
                request.AdminId, request.OrderId, request.Reason);

            var order = await _orderRepository.GetByIdAsync(
                request.OrderId, cancellationToken);

            if (order is null)
                return ResponseDTO.Failure(
                    ErrorCodes.OrderErrors.NotFound,
                    "Order not found");

            try
            {
                order.Cancel(
                    cancelledBy: CancelledBy.Admin,
                    reason: request.Reason,
                    cancelledById: request.AdminId);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDTO.Failure(
                    ErrorCodes.OrderErrors.CannotCancel,
                    ex.Message);
            }

            _unitOfWork.TrackEntity(order);

            _logger.LogInformation(
                "Order {OrderId} cancelled by admin {AdminId}",
                order.Id, request.AdminId);

            return ResponseDTO.Success(new
            {
                order.Id,
                order.OrderCode,
                Status = order.Status.ToString().ToLower(),
                order.CancelReason,
                order.CancelledAt
            }, "Order cancelled successfully");
        }
    }
}
