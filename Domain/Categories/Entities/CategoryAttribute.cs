namespace Daco.Domain.Categories.Entities
{
    public class CategoryAttribute : Entity
    {
        private readonly List<CategoryAttributeValue> _categoryAttributeValues = new();

        public Guid               CategoryId        { get; private set; }
        public string             AttributeName     { get; private set; }
        public string             AttributeSlug     { get; private set; }
        public string?            Description       { get; private set; }
        public AttributeInputType InputType         { get; private set; }
        public string[]           AttributeUnitList { get; private set; }
        public bool               IsRequired        { get; private set; }
        public bool               IsVariation       { get; private set; }
        public int                SortOrder         { get; private set; }
        public string             Unit              { get; private set; }
        public bool               IsActive          { get; private set; }
        public DateTime           CreatedAt         { get; private set; }
        public DateTime?          UpdatedAt         { get; private set; }

        public IReadOnlyCollection<CategoryAttributeValue> CategoryAttributeValues => _categoryAttributeValues.AsReadOnly();

        protected CategoryAttribute() { } 
    }
}
