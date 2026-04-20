namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class UnSuspendProductCommandHandler : IRequestHandler<UnSuspendProductCommand, ResponseDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnSuspendProductCommandHandler> _logger;

        public UnSuspendProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ILogger<UnSuspendProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UnSuspendProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} un suspending product {ProductId}",
                request.UnSuspendedBy, request.ProductId);
            var product = await _productRepository.GetByIdAsync(
                request.ProductId, cancellationToken);

            if (product is null)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.NotFound, "Product not found");

            if (product.Status == ProductStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.AlreadyExists, "Product is already actived");

            if (product.Status != ProductStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.InvalidStatus, $"Cannot un suspend product with status '{product.Status}'");

            product.UnSuspend(request.UnSuspendedBy, request.Reason);
            _unitOfWork.TrackEntity(product);

            _logger.LogInformation(
                "Product {ProductId} un suspended by admin {AdminId}",
                product.Id, request.UnSuspendedBy);

            return ResponseDTO.Success(new
            {
                product.Id,
                product.ShopId,
                Status = product.Status.ToString().ToLower()
            }, "Product un suspended successfully");
        }
    }
}
