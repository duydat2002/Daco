namespace Daco.Application.Catalog.Categories.Commands
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

            var slug = request.CategorySlug;
            if (slug != category.CategorySlug)
            {
                var slugExists = await _categoryRepository.ExistsBySlugAsync(
                    slug,
                    excludeId: request.CategoryId,
                    cancellationToken);

                if (slugExists)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.SlugAlreadyExists, "Slug already in use");
            }

            int level = category.Level;
            Category? parent = null;

            if (request.ParentId != category.ParentId)
            {
                if (request.ParentId == request.CategoryId)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.InvalidParent, "Category cannot be its own parent");

                Category? newParent = null;

                if (request.ParentId.HasValue)
                {
                    newParent = await _categoryRepository.GetByIdAsync(request.ParentId.Value, cancellationToken);

                    if (newParent is null)
                        return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Parent category not found");

                    if (!newParent.IsActive)
                        return ResponseDTO.Failure(ErrorCodes.CategoryErrors.InvalidStatus, "Parent category is not active");

                    if (newParent.Level >= 4)
                        return ResponseDTO.Failure(ErrorCodes.CategoryErrors.LimitExceeded, "Cannot move category deeper than level 4");

                    if (newParent.Path.Contains($"/{request.CategoryId}"))
                        return ResponseDTO.Failure(ErrorCodes.CategoryErrors.InvalidParent, "Cannot move category into its own descendant");

                    newParent.UnmarkLeaf();
                    await _categoryRepository.UpdateAsync(newParent, cancellationToken);
                    _unitOfWork.TrackEntity(newParent);
                }

                if (category.ParentId.HasValue)
                {
                    var oldParent = await _categoryRepository.GetByIdAsync(category.ParentId.Value, cancellationToken);

                    if (oldParent != null)
                    {
                        var siblingsCount = await _categoryRepository.CountChildrenAsync(oldParent.Id, excludeId: request.CategoryId, cancellationToken);

                        if (siblingsCount == 0)
                        {
                            oldParent.MarkAsLeaf();
                            await _categoryRepository.UpdateAsync(oldParent, cancellationToken);
                            _unitOfWork.TrackEntity(oldParent);
                        }
                    }
                }

                // Update level và path của category và toàn bộ con cháu
                var newLevel = newParent is null ? 1 : newParent.Level + 1;
                var newPath = newParent is null ? $"/{category.Id}" : $"{newParent.Path}/{category.Id}";

                // Update path tất cả con cháu
                await _categoryRepository.UpdateDescendantsPathAsync(
                    oldPath: category.Path,
                    newPath: newPath,
                    oldLevel: category.Level,
                    newLevel: newLevel,
                    cancellationToken: cancellationToken);

                category.UpdateParent(request.ParentId, newLevel, newPath);
            }

            category.Update(
                 categoryName: request.CategoryName,
                 categorySlug: slug,
                 description: request.Description,
                 iconUrl: request.IconUrl,
                 imageUrl: request.ImageUrl,
                 sortOrder: request.SortOrder,
                 isActive: request.IsActive);

            await _categoryRepository.UpdateAsync(category, cancellationToken);
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
