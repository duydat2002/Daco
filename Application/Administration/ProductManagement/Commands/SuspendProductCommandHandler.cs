namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class SuspendProductCommandHandler : IRequestHandler<SuspendProductCommand, ResponseDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SuspendProductCommandHandler> _logger;

        public SuspendProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ILogger<SuspendProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SuspendProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} suspending product {ProductId}",
                request.SuspendedBy, request.ProductId);

            var product = await _productRepository.GetByIdAsync(
                request.ProductId, cancellationToken);

            if (product is null)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.NotFound, "Product not found");

            if (product.Status == ProductStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.AlreadyExists, "Product is already suspend");

            if (product.Status != ProductStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.InvalidStatus, $"Cannot approve product with status '{product.Status}'");

            product.Suspend(request.SuspendedBy, request.Reason);
            _unitOfWork.TrackEntity(product);

            _logger.LogInformation(
                "Product {ProductId} suspended by admin {AdminId}",
                product.Id, request.SuspendedBy);

            return ResponseDTO.Success(new
            {
                product.Id,
                product.ShopId,
                Status = product.Status.ToString().ToLower()
            }, "Product suspended successfully");
        }
    }
}
