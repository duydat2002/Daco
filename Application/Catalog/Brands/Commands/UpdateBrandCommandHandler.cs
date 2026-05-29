namespace Daco.Application.Catalog.Brands.Commands
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBrandCommandHandler> _logger;

        public UpdateBrandCommandHandler(
            IBrandRepository brandRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            ILogger<UpdateBrandCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating brand {BrandId}", request.BrandId);

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (brand == null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");

            if (brand.BrandName != request.BrandName)
            {
                var nameExists = await _brandRepository.NameExistsAsync(request.BrandName, brand.Id, cancellationToken);
                if (nameExists)
                    return ResponseDTO.Failure(ErrorCodes.BrandErrors.NameAlreadyExists,
                        "Brand name already exists");
            }

            if (brand.BrandSlug != request.BrandSlug)
            {
                var slugExists = await _brandRepository.SlugExistsAsync(request.BrandSlug, brand.Id, cancellationToken);
                if (slugExists)
                    return ResponseDTO.Failure(ErrorCodes.BrandErrors.SlugAlreadyExists,
                        "Brand slug already exists");
            }

            var logoUrl = request.LogoUrl;
            if (brand.LogoUrl != request.LogoUrl)
            {
                if (!string.IsNullOrEmpty(request.LogoUrl))
                {
                    try
                    {
                        logoUrl = await _fileStorageService.MoveBrandImageAsync(
                            "logo",
                            request.LogoUrl,
                            brand.Id,
                            cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to move logo {TempUrl} for brand {BrandId}", request.LogoUrl, brand.Id);
                    }
                }

                try
                {
                    await _fileStorageService.DeleteAsync(brand.LogoUrl, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete image {ImageUrl}", brand.LogoUrl);
                }
            }

            brand.Update(
               brandName: request.BrandName,
               brandSlug: request.BrandSlug,
               description: request.Description,
               websiteUrl: request.WebsiteUrl,
               logoUrl: logoUrl);

            var addedImages = request.SampleImages.Except(brand.SampleImages).ToList();

            var removedImages = brand.SampleImages.Except(request.SampleImages).ToList();

            foreach (var imageInput in addedImages)
            {
                try
                {
                    var permanentUrl = await _fileStorageService.MoveBrandImageAsync(
                    "sample",
                    imageInput,
                    brand.Id,
                    cancellationToken);

                    brand.AddSample(permanentUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to move sample {TempUrl} for brand {BrandId}", request.LogoUrl, brand.Id);
                }
            }

            foreach (var imageInput in removedImages)
            {
                try
                {
                    await _fileStorageService.DeleteAsync(imageInput, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete image {ImageUrl}", imageInput);
                }

                brand.DeleteSample(imageInput);
            }

            await _brandRepository.UpdateAsync(brand, cancellationToken);

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
