namespace Daco.Domain.Categories.Entities
{
    public class CategoryAttributeValue : Entity
    {
        private readonly List<CategoryAttribute> _categoryAttributes = new();

        public Guid     AttributeId { get; private set; }
        public string   Value       { get; private set; }
        public int      SortOrder   { get; private set; }
        public bool     IsActive    { get; private set; }
        public DateTime CreatedAt   { get; private set; }

        public IReadOnlyCollection<CategoryAttribute> CategoryAttributes => _categoryAttributes.AsReadOnly();


        protected CategoryAttributeValue() { }
    }
}
