namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class CreateCategoryAttributeCommandHandler : IRequestHandler<CreateCategoryAttributeCommand, ResponseDTO>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAttributeRepository _attributeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCategoryAttributeCommandHandler> _logger;

        public CreateCategoryAttributeCommandHandler(
            ICategoryRepository categoryRepository,
            ICategoryAttributeRepository attributeRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateCategoryAttributeCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CreateCategoryAttributeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Admin {AdminId} creating attribute '{AttributeName}' for category {CategoryId}",
                request.CreatedBy, request.AttributeName, request.CategoryId);

            var category = await _categoryRepository.GetByIdAsync(
                request.CategoryId, cancellationToken);

            if (category is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.NotFound,
                    "Category not found");

            if (!category.IsActive)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.IsInactive,
                    "Cannot add attribute to an inactive category");

            var slugExists = await _attributeRepository.SlugExistsInCategoryAsync(
                request.CategoryId,
                request.AttributeSlug,
                null,
                cancellationToken);

            if (slugExists)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.SlugAlreadyExists,
                    $"Attribute slug '{request.AttributeSlug}' already exists in this category");

            var attribute = CategoryAttribute.Create(
                categoryId: request.CategoryId,
                attributeName: request.AttributeName,
                attributeSlug: request.AttributeSlug,
                inputType: request.InputType,
                isRequired: request.IsRequired,
                isVariation: request.IsVariation,
                sortOrder: request.SortOrder,
                description: request.Description,
                unit: request.Unit,
                attributeUnitList: request.AttributeUnitList);

            if (request.PredefinedValues is { Count: > 0 })
            {
                var sortOrder = 0;
                foreach (var value in request.PredefinedValues.Distinct())
                {
                    var attrValue = CategoryAttributeValue.Create(
                        attributeId: attribute.Id,
                        value: value.Trim(),
                        sortOrder: sortOrder++);

                    attribute.AddValue(attrValue);
                }
            }

            await _attributeRepository.AddAsync(attribute, cancellationToken);

            _logger.LogInformation("Attribute {AttributeId} '{AttributeName}' created for category {CategoryId}",
                attribute.Id, attribute.AttributeName, request.CategoryId);

            return ResponseDTO.Success(new
            {
                attribute.Id,
                attribute.CategoryId,
                attribute.AttributeName,
                attribute.AttributeSlug,
                InputType = attribute.InputType.ToString().ToLower(),
                attribute.IsRequired,
                attribute.IsVariation,
                attribute.SortOrder,
                attribute.Unit,
                attribute.AttributeUnitList,
                attribute.IsActive,
                attribute.CreatedAt,
                PredefinedValues = attribute.CategoryAttributeValues
                    .Select(v => new { v.Id, v.Value, v.SortOrder })
                    .ToList()
            }, "Attribute created successfully");
        }
    }
}
