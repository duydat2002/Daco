namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class UpdateCategoryAttributeCommandHandler : IRequestHandler<UpdateCategoryAttributeCommand, ResponseDTO>
    {
        private readonly ICategoryAttributeRepository _attributeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCategoryAttributeCommandHandler> _logger;

        public UpdateCategoryAttributeCommandHandler(
            ICategoryAttributeRepository attributeRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateCategoryAttributeCommandHandler> logger)
        {
            _attributeRepository = attributeRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateCategoryAttributeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating category attribute {AttributeId}", request.AttributeId);

            var attribute = await _attributeRepository.GetByIdWithValuesAsync(
                request.AttributeId, cancellationToken);

            if (attribute is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.AttributeNotFound,
                    "Category attribute not found");

            var slugExists = await _attributeRepository.SlugExistsInCategoryAsync(
                attribute.CategoryId,
                request.AttributeSlug,
                excludeId: request.AttributeId,
                cancellationToken);

            if (slugExists)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.AttributeSlugAlreadyExists,
                    "Attribute slug already exists in this category");

            attribute.Update(
                attributeName: request.AttributeName,
                attributeSlug: request.AttributeSlug,
                inputType: request.InputType,
                isRequired: request.IsRequired,
                isVariation: request.IsVariation,
                sortOrder: request.SortOrder,
                description: request.Description,
                unit: request.Unit,
                attributeUnitList: request.AttributeUnitList);

            if (request.PredefinedValues is not null)
            {
                attribute.SyncValues(
                    request.PredefinedValues
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .Select(v => v.Trim())
                        .Distinct()
                        .ToList());
            }

            _logger.LogInformation("Category attribute {AttributeId} updated successfully", request.AttributeId);

            return ResponseDTO.Success(new
            {
                attribute.Id,
                attribute.CategoryId,
                attribute.AttributeName,
                attribute.AttributeSlug,
                attribute.InputType,
                attribute.IsRequired,
                attribute.IsVariation,
                attribute.SortOrder,
                attribute.IsActive,
                attribute.UpdatedAt,
                PredefinedValues = attribute.CategoryAttributeValues
                    .Where(v => v.IsActive)
                    .OrderBy(v => v.SortOrder)
                    .Select(v => v.Value)
            }, "Category attribute updated successfully");
        }
    }
}
