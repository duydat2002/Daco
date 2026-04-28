namespace Daco.Application.Administration.BrandManagement.Commands
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBrandCommandHandler> _logger;

        public DeleteBrandCommandHandler(
            IBrandRepository brandRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteBrandCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting brand {BrandId}", request.BrandId);

            var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
            if (brand is null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");

            var hasProducts = await _brandRepository.HasProductsAsync(request.BrandId, cancellationToken);

            if (hasProducts)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.HasProducts,
                    "Cannot delete brand that has products");

            _brandRepository.Delete(brand);

            _logger.LogInformation("Brand {BrandId} deleted successfully", request.BrandId);

            return ResponseDTO.Success(null, "Brand deleted successfully");
        }
    }
}
