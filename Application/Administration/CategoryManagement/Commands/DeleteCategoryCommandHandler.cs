namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseDTO>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteCategoryCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting category {CategoryId}", request.CategoryId);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (category is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Category not found");

            var hasChildren = await _categoryRepository.HasChildrenAsync(request.CategoryId, cancellationToken);
            if (hasChildren)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.HasChildren,
                    "Cannot delete category that has subcategories");

            var hasProducts = await _categoryRepository.HasProductsAsync(request.CategoryId, cancellationToken);
            if (hasProducts)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.HasProducts,
                    "Cannot delete category that has products");

            _categoryRepository.Delete(category);

            _logger.LogInformation("Category {CategoryId} deleted successfully", request.CategoryId);

            return ResponseDTO.Success(null, "Category deleted successfully");
        }
    }
}
