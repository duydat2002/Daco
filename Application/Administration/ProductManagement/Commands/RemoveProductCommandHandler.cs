namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, ResponseDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveProductCommandHandler> _logger;

        public RemoveProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} remove product {ProductId}",
                request.RemovedBy, request.ProductId);

            var product = await _productRepository.GetByIdAsync(
                request.ProductId, cancellationToken);

            if (product is null)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.NotFound, "Product not found");

            if (product.Status == ProductStatus.Deleted)
                return ResponseDTO.Failure(ErrorCodes.ProductErrors.AlreadyExists, "Product is already suspend");

            //if (product.Status != ProductStatus.Active)
            //    return ResponseDTO.Failure(ErrorCodes.ProductErrors.InvalidStatus, $"Cannot approve product with status '{product.Status}'");

            product.Remove(request.RemovedBy, request.Reason);
            _unitOfWork.TrackEntity(product);

            _logger.LogInformation(
                "Product {ProductId} removed by admin {AdminId}",
                product.Id, request.RemovedBy);

            return ResponseDTO.Success(new
            {
                product.Id,
                product.ShopId,
                Status = product.Status.ToString().ToLower()
            }, "Product suspended successfully");
        }
    }
}
