namespace Daco.Domain.Categories.Entities
{
    public class CategoryAttributeValue : Entity
    {
        public Guid     AttributeId { get; private set; }
        public string   Value       { get; private set; }
        public int      SortOrder   { get; private set; }
        public bool     IsActive    { get; private set; }
        public DateTime CreatedAt   { get; private set; }

        protected CategoryAttributeValue() { }
    }
}
