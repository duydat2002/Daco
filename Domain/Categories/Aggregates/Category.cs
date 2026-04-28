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

        public static Category Create(
            string  categoryName,
            string  categorySlug,
            int     level,
            Guid?   parentId = null,
            string? description = null,
            string? path = null,
            string? iconUrl = null,
            string? imageUrl = null,
            int     sortOrder = 0,
            bool    isLeaf = false)
        {
            Guard.AgainstNullOrEmpty(categoryName, nameof(categoryName));
            Guard.AgainstNullOrEmpty(categorySlug, nameof(categorySlug));
            Guard.AgainstOutOfRange(level, 1, 4, nameof(level));

            return new Category
            {
                Id           = Guid.NewGuid(),
                ParentId     = parentId ?? Guid.Empty,
                CategoryName = categoryName.Trim(),
                CategorySlug = categorySlug.Trim().ToLowerInvariant(),
                Description  = description,
                Level        = level,
                Path         = path,
                IconUrl      = iconUrl,
                ImageUrl     = imageUrl,
                SortOrder    = sortOrder,
                IsActive     = true,
                IsLeaf       = isLeaf,
                ProductCount = 0,
                CreatedAt    = DateTime.UtcNow
            };
        }

        public void Update(
            string categoryName,
            string categorySlug,
            string? description = null,
            string? iconUrl = null,
            string? imageUrl = null,
            int sortOrder = 0)
        {
            Guard.AgainstNullOrEmpty(categoryName, nameof(categoryName));
            Guard.AgainstNullOrEmpty(categorySlug, nameof(categorySlug));

            CategoryName = categoryName.Trim();
            CategorySlug = categorySlug.Trim().ToLowerInvariant();
            Description = description;
            IconUrl = iconUrl;
            ImageUrl = imageUrl;
            SortOrder = sortOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
