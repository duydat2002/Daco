namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public class AddAttributeValueCommandHandler : IRequestHandler<AddAttributeValueCommand, ResponseDTO>
    {
        private readonly ICategoryAttributeRepository _attributeRepository;
        private readonly ILogger<AddAttributeValueCommandHandler> _logger;

        public AddAttributeValueCommandHandler(
            ICategoryAttributeRepository attributeRepository,
            ILogger<AddAttributeValueCommandHandler> logger)
        {
            _attributeRepository = attributeRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AddAttributeValueCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding values to attribute {AttributeId}", request.AttributeId);

            var attribute = await _attributeRepository.GetByIdWithValuesAsync(
                request.AttributeId, cancellationToken);

            if (attribute is null)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.AttributeNotFound,
                    "Category attribute not found");

            if (attribute.InputType != AttributeInputType.Select &&
                attribute.InputType != AttributeInputType.MultiSelect)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.InvalidInputType,
                    "Predefined values only allowed for Select and MultiSelect input types");

            var existingValues = attribute.CategoryAttributeValues
                .Select(v => v.Value)
                .ToHashSet();

            var newValues = request.Values
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v.Trim())
                .Distinct()
                .Where(v => !existingValues.Contains(v))
                .ToList();

            if (newValues.Count == 0)
                return ResponseDTO.Failure(ErrorCodes.CategoryErrors.AttributeValueAlreadyExists,
                    "All provided values already exist");

            var sortOrder = attribute.CategoryAttributeValues.Count;
            foreach (var value in newValues)
            {
                var attrValue = CategoryAttributeValue.Create(
                    attributeId: attribute.Id,
                    value: value,
                    sortOrder: sortOrder++);

                attribute.AddValue(attrValue);
            }

            _logger.LogInformation("Added {Count} values to attribute {AttributeId}",
                newValues.Count, request.AttributeId);

            return ResponseDTO.Success(new
            {
                attribute.Id,
                attribute.AttributeName,
                AddedValues = newValues,
                AllValues = attribute.CategoryAttributeValues
                    .Where(v => v.IsActive)
                    .OrderBy(v => v.SortOrder)
                    .Select(v => new { v.Id, v.Value, v.SortOrder })
            }, $"Added {newValues.Count} value(s) successfully");
        }
    }
}
