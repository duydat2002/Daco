namespace Daco.Application.Catalog.Brands.Commands
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBrandCommandHandler> _logger;

        public CreateBrandCommandHandler(
            IBrandRepository brandRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            ILogger<CreateBrandCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating brand {BrandName}", request.BrandName);

            var nameExists = await _brandRepository.NameExistsAsync(request.BrandName, null, cancellationToken);
            if (nameExists)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NameAlreadyExists, "Brand name already exists");

            var slug = SlugGenerator.FromName(request.BrandName);
            var attempt = 0;

            while (await _brandRepository.SlugExistsAsync(slug, null, cancellationToken))
                slug = SlugGenerator.WithSuffix(request.BrandName, ++attempt);

            var brand = Brand.Create(
                brandName:    request.BrandName,
                brandSlug:    slug,
                description:  request.Description,
                websiteUrl:   request.WebsiteUrl);

            if (!string.IsNullOrEmpty(request.LogoUrl))
            {
                try
                {
                    var permanentLogoUrl = await _fileStorageService.MoveBrandImageAsync(
                        "logo",
                        request.LogoUrl,
                        brand.Id,
                        cancellationToken);

                    brand.AddLogo(permanentLogoUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to move logo {TempUrl} for brand {BrandId}", request.LogoUrl, brand.Id);
                }
            }

            if (request.SampleImages.Any())
            {
                foreach (var imageInput in request.SampleImages)
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
            }

            await _brandRepository.AddAsync(brand, cancellationToken);

            _logger.LogInformation("Brand {BrandId} created successfully", brand.Id);

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
                brand.CreatedAt
            }, "Brand created successfully");
        }
    }
}
