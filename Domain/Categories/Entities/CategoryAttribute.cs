namespace Daco.Domain.Categories.Entities
{
    public class CategoryAttribute : Entity
    {
        private readonly List<CategoryAttributeValue> _categoryAttributeValues = new();

        public Guid                CategoryId        { get; private set; }
        public string              AttributeName     { get; private set; }
        public string              AttributeSlug     { get; private set; }
        public string?             Description       { get; private set; }
        public AttributeInputType  InputType         { get; private set; }
        public string[]?           AttributeUnitList { get; private set; }
        public bool                IsRequired        { get; private set; }
        public bool                IsVariation       { get; private set; }
        public int                 SortOrder         { get; private set; }
        public string?             Unit              { get; private set; }
        public bool                IsActive          { get; private set; }
        public DateTime            CreatedAt         { get; private set; }
        public DateTime?           UpdatedAt         { get; private set; }

        public IReadOnlyCollection<CategoryAttributeValue> CategoryAttributeValues => _categoryAttributeValues.AsReadOnly();

        protected CategoryAttribute() { }

        public                 static CategoryAttribute Create(
            Guid               categoryId,
            string             attributeName,
            string             attributeSlug,
            AttributeInputType inputType,
            bool               isRequired = false,
            bool               isVariation = false,
            int                sortOrder = 0,
            string?            description = null,
            string?            unit = null,
            string[]?          attributeUnitList = null)
        {
            Guard.AgainstNullOrEmpty(attributeName, nameof(attributeName));
            Guard.AgainstNullOrEmpty(attributeSlug, nameof(attributeSlug));
            Guard.Against(categoryId == Guid.Empty, "CategoryId is required");

            return new CategoryAttribute
            {
                Id                = Guid.NewGuid(),
                CategoryId        = categoryId,
                AttributeName     = attributeName.Trim(),
                AttributeSlug     = attributeSlug.Trim().ToLowerInvariant(),
                InputType         = inputType,
                IsRequired        = isRequired,
                IsVariation       = isVariation,
                SortOrder         = sortOrder,
                Description       = description,
                Unit              = unit,
                AttributeUnitList = attributeUnitList ?? Array.Empty<string>(),
                IsActive          = true,
                CreatedAt         = DateTime.UtcNow
            };
        }

        public void AddValue(CategoryAttributeValue value)
        {
            Guard.AgainstNull(value, nameof(value));
            Guard.Against(
                InputType != AttributeInputType.Select
                && InputType != AttributeInputType.MultiSelect,
                "Only Select/MultiSelect attributes can have predefined values");

            Guard.Against(
                _categoryAttributeValues.Any(v => v.Value == value.Value),
                $"Value '{value.Value}' already exists");

            _categoryAttributeValues.Add(value);
        }

        public void Update(
            string attributeName,
            string attributeSlug,
            AttributeInputType inputType,
            bool isRequired,
            bool isVariation,
            int sortOrder,
            string? description = null,
            string? unit = null,
            string[]? attributeUnitList = null)
        {
            Guard.AgainstNullOrEmpty(attributeName, nameof(attributeName));
            Guard.AgainstNullOrEmpty(attributeSlug, nameof(attributeSlug));
            Guard.Against(
                isVariation && inputType != AttributeInputType.Select && inputType != AttributeInputType.MultiSelect,
                "Variation attribute must be Select or MultiSelect input type");

            AttributeName = attributeName.Trim();
            AttributeSlug = attributeSlug.Trim().ToLowerInvariant();
            InputType = inputType;
            IsRequired = isRequired;
            IsVariation = isVariation;
            SortOrder = sortOrder;
            Description = description;
            Unit = unit;
            AttributeUnitList = attributeUnitList;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SyncValues(List<string> newValues)
        {
            Guard.Against(
                InputType != AttributeInputType.Select && InputType != AttributeInputType.MultiSelect,
                "Predefined values only allowed for Select and MultiSelect input types");

            var toRemove = _categoryAttributeValues
                .Where(v => !newValues.Contains(v.Value))
                .ToList();

            foreach (var value in toRemove)
                _categoryAttributeValues.Remove(value);

            var existingValues = _categoryAttributeValues.Select(v => v.Value).ToHashSet();
            var sortOrder = _categoryAttributeValues.Count;

            foreach (var value in newValues.Where(v => !existingValues.Contains(v)))
            {
                var attrValue = CategoryAttributeValue.Create(
                    attributeId: Id,
                    value: value,
                    sortOrder: sortOrder++);

                _categoryAttributeValues.Add(attrValue);
            }

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
