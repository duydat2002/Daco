namespace Daco.Application.Catalog.Brands.Commands
{
    public class RestoreBrandCommandHandler : IRequestHandler<RestoreBrandCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RestoreBrandCommandHandler> _logger;

        public RestoreBrandCommandHandler(
            IBrandRepository brandRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<RestoreBrandCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RestoreBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring brand {BrandId}", request.BrandId);

            var brand = await _brandRepository.GetByIdIncludeDeletedAsync(request.BrandId, cancellationToken);
            if (brand is null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");

            if (brand.DeletedAt == null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.AlreadyActive, "Brand already actived");

            brand.Restore();

            await _brandRepository.UpdateAsync(brand, cancellationToken);

            _logger.LogInformation("Brand {BrandId} restored successfully", request.BrandId);

            return ResponseDTO.Success(null, "Brand restored successfully");
        }
    }
}
