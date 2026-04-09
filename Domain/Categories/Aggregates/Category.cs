namespace Daco.Domain.Categories.Aggregates
{
    public class Category : AggregateRoot
    {
        private readonly List<CategoryAttribute> _categoryAttributes = new();

        public Guid      ParentId     { get; private set; }
        public string    CategoryName { get; private set; }
        public string    CategorySlug { get; private set; }
        public string?   Description  { get; private set; }
        public int       Level        { get; private set; }
        public string?   Path         { get; private set; }
        public string?   IconUrl      { get; private set; }
        public string?   ImageUrl     { get; private set; }
        public int       SortOrder    { get; private set; }
        public bool      IsActive     { get; private set; }
        public bool      IsLeaf       { get; private set; }
        public int       ProductCount { get; private set; }
        public DateTime  CreatedAt    { get; private set; }
        public DateTime? UpdatedAt    { get; private set; }

        public IReadOnlyCollection<CategoryAttribute> CategoryAttributes => _categoryAttributes.AsReadOnly();

        protected Category() { }


    }
}
