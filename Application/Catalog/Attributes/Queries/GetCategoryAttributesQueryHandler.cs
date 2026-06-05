using Daco.Application.Catalog.Attributes.DTOs;

namespace Daco.Application.Catalog.Attributes.Queries
{
    public class GetCategoryAttributesQueryHandler : IRequestHandler<GetCategoryAttributesQuery, ResponseDTO>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly ILogger<GetCategoryAttributesQueryHandler> _logger;

        public GetCategoryAttributesQueryHandler(
            ICategoryRepository categoryRepository,
            ICategoryAttributeRepository categoryAttributeRepository,
            ILogger<GetCategoryAttributesQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetCategoryAttributesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting attributes for category {CategoryId}", request.CategoryId);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

            if (category is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Category not found");

            if (!category.IsLeaf)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotLeaf,
                    "Attributes only available for leaf categories");

            var attributes = await _categoryAttributeRepository.GetByCategoryIdAsync(category.Id);

            var result = attributes
                .Where(a => a.IsActive)
                .OrderBy(a => a.SortOrder)
                .Select(a => new CategoryAttributeDTO
                {
                    Id            = a.Id,
                    AttributeName = a.AttributeName,
                    AttributeSlug = a.AttributeSlug,
                    InputType     = a.InputType.ToString().ToLower(),
                    IsRequired    = a.IsRequired,
                    IsVariation   = a.IsVariation,
                    SortOrder     = a.SortOrder,
                    Unit          = a.Unit,
                    UnitList      = a.AttributeUnitList,
                    Options       = a.CategoryAttributeValues
                        .Where(v => v.IsActive)
                        .OrderBy(v => v.SortOrder)
                        .Select(v => new AttributeValueOptionDTO
                        {
                            Id = v.Id,
                            Value = v.Value,
                            SortOrder = v.SortOrder
                        })
                        .ToList()
                })
                .ToList();

            return ResponseDTO.Success(new
            {
                CategoryId = category.Id,
                CategoryName = category.CategoryName,
                Attributes = result
            });
        }
    }
}
