namespace Daco.Application.Administration.BrandManagement.Commands
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBrandCommandHandler> _logger;

        public UpdateBrandCommandHandler(
            IBrandRepository brandRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateBrandCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating brand {BrandId}", request.BrandId);

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (brand == null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");

            var nameExists = await _brandRepository.NameExistsAsync(request.BrandName, null, cancellationToken);
            if (nameExists)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NameAlreadyExists,
                    "Brand name already exists");

            var slugExists = await _brandRepository.SlugExistsAsync(request.BrandSlug, null, cancellationToken);
            if (slugExists)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.SlugAlreadyExists,
                    "Brand slug already exists");

            brand.Update(
                brandName: request.BrandName,
                brandSlug: request.BrandSlug,
                description: request.Description,
                websiteUrl: request.WebsiteUrl,
                logoUrl: request.LogoUrl,
                sampleImages: request.SampleImages);

            await _brandRepository.AddAsync(brand, cancellationToken);

            _logger.LogInformation("Brand {BrandId} updated successfully", brand.Id);

            return ResponseDTO.Success(new
            {
                brand.Id,
                brand.BrandName,
                brand.BrandSlug,
                brand.Description,
                brand.WebsiteUrl,
                brand.LogoUrl,
                brand.SampleImages,
                brand.IsActive,
                brand.CreatedAt,
                brand.UpdatedAt
            }, "Brand updated successfully");
        }
    }
}
