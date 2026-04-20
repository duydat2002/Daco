namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class ApproveProductCommandHandler : IRequestHandler<ApproveProductCommand, ResponseDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApproveProductCommandHandler> _logger;

        public ApproveProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ILogger<ApproveProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(ApproveProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} approving product {ProductId}",
                request.ApprovedBy, request.ProductId);

            var product = await _productRepository.GetByIdAsync(
                request.ProductId, cancellationToken);

            if (product is null)
                return ResponseDTO.Failure(
                    ErrorCodes.ProductErrors.NotFound,
                    "Product not found");

            if (product.Status == ProductStatus.Active)
                return ResponseDTO.Failure(
                    ErrorCodes.ProductErrors.AlreadyExists,
                    "Product is already active");

            if (product.Status != ProductStatus.Pending)
                return ResponseDTO.Failure(
                    ErrorCodes.ProductErrors.InvalidStatus, 
                    $"Cannot approve product with status '{product.Status}'");

            product.Approve(request.ApprovedBy);
            _unitOfWork.TrackEntity(product);

            _logger.LogInformation(
                "Product {ProductId} approved by admin {AdminId}",
                product.Id, request.ApprovedBy);

            return ResponseDTO.Success(new
            {
                product.Id,
                product.ShopId,
                Status = product.Status.ToString().ToLower(),
                product.PublishedAt
            }, "Product approved successfully");
        }
    }
}
