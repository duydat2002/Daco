namespace Daco.Application.Administration.BrandManagement.Commands
{
    public class AssignBrandToCategoryCommandHandler : IRequestHandler<AssignBrandToCategoryCommand, ResponseDTO>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignBrandToCategoryCommandHandler> _logger;

        public AssignBrandToCategoryCommandHandler(
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignBrandToCategoryCommandHandler> logger)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AssignBrandToCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Assigning brand {BrandId} to {Count} categories",
                request.BrandId, request.CategoryIds.Count);

            var brand = await _brandRepository.GetByIdWithCategoriesAsync(
                request.BrandId, cancellationToken);

            if (brand is null)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.NotFound, "Brand not found");

            var categories = await _categoryRepository.GetByIdsAsync(
                request.CategoryIds, cancellationToken);

            var notFound = request.CategoryIds
                .Except(categories.Select(c => c.Id))
                .ToList();

            if (notFound.Any())
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound,
                    $"Categories not found: {string.Join(", ", notFound)}");

            var nonLeaf = categories.Where(c => !c.IsLeaf).ToList();
            if (nonLeaf.Any())
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotLeaf,
                    $"Only leaf categories can be assigned to brands: {string.Join(", ", nonLeaf.Select(c => c.CategoryName))}");

            var alreadyAssigned = brand.BrandCategories
                .Select(bc => bc.CategoryId)
                .ToHashSet();

            var toAssign = request.CategoryIds
                .Where(id => !alreadyAssigned.Contains(id))
                .ToList();

            if (toAssign.Count == 0)
                return ResponseDTO.Failure(ErrorCodes.BrandErrors.AlreadyAssigned,
                    "All provided categories are already assigned to this brand");

            foreach (var categoryId in toAssign)
                brand.AssignCategory(categoryId);

            _unitOfWork.TrackEntity(brand);

            _logger.LogInformation("Assigned {Count} categories to brand {BrandId}",
                toAssign.Count, request.BrandId);

            return ResponseDTO.Success(new
            {
                brand.Id,
                brand.BrandName,
                AssignedCategories = toAssign,
                TotalCategories = brand.BrandCategories.Count
            }, $"Assigned {toAssign.Count} category(s) to brand successfully");
        }
    }
}
