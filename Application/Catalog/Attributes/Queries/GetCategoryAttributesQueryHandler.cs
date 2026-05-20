using Daco.Application.Catalog.Attributes.DTOs;

namespace Daco.Application.Catalog.Attributes.Queries
{
    public class GetCategoryAttributesQueryHandler : IRequestHandler<GetCategoryAttributesQuery, ResponseDTO>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<GetCategoryAttributesQueryHandler> _logger;

        public GetCategoryAttributesQueryHandler(
            ICategoryRepository categoryRepository,
            ILogger<GetCategoryAttributesQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetCategoryAttributesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting attributes for category {CategoryId}", request.CategoryId);

            var category = await _categoryRepository.GetByIdWithAttributesAsync(request.CategoryId, cancellationToken);

            if (category is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound, "Category not found");

            if (!category.IsLeaf)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotLeaf,
                    "Attributes only available for leaf categories");

            var result = category.CategoryAttributes
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
