namespace Daco.Application.Administration.CategoryManagement.Commands
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

            var slugExists = await _categoryRepository.ExistsBySlugAsync(request.CategorySlug, null, cancellationToken);

            if (slugExists)
                return ResponseDTO.Failure(
                    ErrorCodes.CategoryErrors.SlugAlreadyExists, $"Slug '{request.CategorySlug}' is already taken");

            int level = 1;
            string? path = null;
            bool isLeaf = request.IsLeaf;

            if (request.ParentId.HasValue)
            {
                var parent = await _categoryRepository.GetByIdAsync(request.ParentId.Value, cancellationToken);

                if (parent is null)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Parent category not found");

                if (!parent.IsActive)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.InvalidStatus, "Parent category is not active");

                if (parent.Level >= 4)
                    return ResponseDTO.Failure(ErrorCodes.CategoryErrors.LimitExceeded, "Cannot create category deeper than level 4");

                level = parent.Level + 1;
                path = string.IsNullOrEmpty(parent.Path)
                    ? $"/{parent.Id}"
                    : $"{parent.Path}/{parent.Id}";
            }

            var category = Category.Create(
                categoryName: request.CategoryName,
                categorySlug: request.CategorySlug,
                level: level,
                parentId: request.ParentId,
                description: request.Description,
                path: path,
                iconUrl: request.IconUrl,
                imageUrl: request.ImageUrl,
                sortOrder: request.SortOrder,
                isLeaf: isLeaf);

            await _categoryRepository.AddAsync(category, cancellationToken);
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
