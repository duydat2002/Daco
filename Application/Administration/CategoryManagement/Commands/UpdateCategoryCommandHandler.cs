namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseDTO>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateCategoryCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating category {CategoryId}", request.CategoryId);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (category is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Category not found");

            var slugExists = await _categoryRepository.ExistsBySlugAsync(
                request.CategorySlug,
                excludeId: request.CategoryId,
                cancellationToken);

            if (slugExists)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.SlugAlreadyExists, "Slug already in use");

            category.Update(
                categoryName: request.CategoryName,
                categorySlug: request.CategorySlug,
                description: request.Description,
                iconUrl: request.IconUrl,
                imageUrl: request.ImageUrl,
                sortOrder: request.SortOrder);

            if (category.IsActive != request.IsActive)
                category.SetActive(request.IsActive);

            _unitOfWork.TrackEntity(category);

            _logger.LogInformation("Category {CategoryId} updated successfully", request.CategoryId);

            return ResponseDTO.Success(new
            {
                category.Id,
                category.CategoryName,
                category.CategorySlug,
                category.Description,
                category.Level,
                category.IsActive,
                category.IsLeaf,
                category.SortOrder,
                category.UpdatedAt
            }, "Category updated successfully");
        }
    }
}
