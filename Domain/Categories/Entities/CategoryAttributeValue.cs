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

        public static CategoryAttributeValue Create(
            Guid attributeId,
            string value,
            int sortOrder = 0)
        {
            Guard.AgainstNullOrEmpty(value, nameof(value));
            Guard.Against(attributeId == Guid.Empty, "AttributeId is required");

            return new CategoryAttributeValue
            {
                Id = Guid.NewGuid(),
                AttributeId = attributeId,
                Value = value.Trim(),
                SortOrder = sortOrder,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
