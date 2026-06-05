using Daco.Domain.Orders.Entities;

namespace Daco.Application.Catalog.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseDTO>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateCategoryCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {AdminId} creating category '{CategoryName}'",
                request.CreatedBy, request.CategoryName);

            var slug = SlugGenerator.FromName(request.CategoryName);
            var attempt = 0;

            while (await _categoryRepository.ExistsBySlugAsync(slug, null, cancellationToken))
                slug = SlugGenerator.WithSuffix(request.CategoryName, ++attempt);

            int level = 1;
            Category? parent = null;

            if (request.ParentId.HasValue)
            {
                parent = await _categoryRepository.GetByIdAsync(request.ParentId.Value, cancellationToken);

                if (parent is null)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Parent category not found");

                if (!parent.IsActive)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.InvalidStatus, "Parent category is not active");

                if (parent.Level >= 4)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.LimitExceeded, "Cannot create category deeper than level 4");

                level = parent.Level + 1;
                
                parent.UnmarkLeaf();
            }

            var category = Category.Create(
                categoryName: request.CategoryName,
                categorySlug: slug,
                level: level,
                parentId: request.ParentId,
                description: request.Description,
                iconUrl: request.IconUrl,   
                imageUrl: request.ImageUrl,
                sortOrder: request.SortOrder);

            category.SetPath(parent == null ? $"/{category.Id}" : $"{parent.Path}/{category.Id}");

            await _categoryRepository.AddAsync(category, cancellationToken);
            if (parent != null)
            {
                await _categoryRepository.UpdateAsync(parent, cancellationToken);
                _unitOfWork.TrackEntity(parent);
            }
            _unitOfWork.TrackEntity(category);

            _logger.LogInformation("Category {CategoryId} '{CategoryName}' created at level {Level}",
                category.Id, category.CategoryName, category.Level);

            return ResponseDTO.Success(new
            {
                category.Id,
                category.CategoryName,
                category.CategorySlug,
                category.Level,
                category.ParentId,
                category.Path,
                category.IsLeaf,
                category.SortOrder,
                category.IsActive,
                category.CreatedAt
            }, "Category created successfully");
        }
    }
}
