namespace Daco.Application.Administration.BrandManagement.Commands
{
    public class UnassignBrandFromCategoryCommandHandler : IRequestHandler<UnassignBrandFromCategoryCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnassignBrandFromCategoryCommandHandler> _logger;

        public UnassignBrandFromCategoryCommandHandler(
            IBrandRepository brandRepository,
            IUnitOfWork unitOfWork,
            ILogger<UnassignBrandFromCategoryCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UnassignBrandFromCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unassigning {Count} categories from brand {BrandId}",
                request.CategoryIds.Count, request.BrandId);

            var brand = await _brandRepository.GetByIdWithCategoriesAsync(
                request.BrandId, cancellationToken);

            if (brand is null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");

            var assignedIds = brand.BrandCategories
                .Select(bc => bc.CategoryId)
                .ToHashSet();

            var notAssigned = request.CategoryIds
                .Where(id => !assignedIds.Contains(id))
                .ToList();

            if (notAssigned.Any())
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotAssigned,
                    $"Categories not assigned to this brand: {string.Join(", ", notAssigned)}");

            foreach (var categoryId in request.CategoryIds)
                brand.UnassignCategory(categoryId);

            _unitOfWork.TrackEntity(brand);

            _logger.LogInformation("Unassigned {Count} categories from brand {BrandId}",
                request.CategoryIds.Count, request.BrandId);

            return ResponseDTO.Success(new
            {
                brand.Id,
                brand.BrandName,
                UnassignedCategories = request.CategoryIds,
                TotalCategories = brand.BrandCategories.Count
            }, $"Unassigned {request.CategoryIds.Count} category(s) from brand successfully");
        }
    }
}
