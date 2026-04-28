namespace Daco.Application.Administration.BrandManagement.Commands
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBrandCommandHandler> _logger;

        public CreateBrandCommandHandler(
            IBrandRepository brandRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateBrandCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating brand {BrandName}", request.BrandName);

            var nameExists = await _brandRepository.NameExistsAsync(request.BrandName, null, cancellationToken);
            if (nameExists)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NameAlreadyExists,
                    "Brand name already exists");

            var slugExists = await _brandRepository.SlugExistsAsync(request.BrandSlug, null, cancellationToken);
            if (slugExists)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.SlugAlreadyExists,
                    "Brand slug already exists");

            var brand = Brand.Create(
                brandName: request.BrandName,
                brandSlug: request.BrandSlug,
                description: request.Description,
                websiteUrl: request.WebsiteUrl,
                logoUrl: request.LogoUrl,
                sampleImages: request.SampleImages);

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
