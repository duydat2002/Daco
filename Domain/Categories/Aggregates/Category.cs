namespace Daco.Domain.Categories.Aggregates
{
    public class Category : AggregateRoot
    {
        public Guid?     ParentId     { get; private set; } = null;
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

        protected Category() { }

        public static Category Create(
            string  categoryName,
            string  categorySlug,
            int     level,
            Guid?   parentId    = null,
            string? description = null,
            string? iconUrl     = null,
            string? imageUrl    = null,
            int     sortOrder   = 0)
        {
            Guard.AgainstNullOrEmpty(categoryName, nameof(categoryName));
            Guard.AgainstNullOrEmpty(categorySlug, nameof(categorySlug));
            Guard.AgainstOutOfRange(level, 1, 4, nameof(level));

            return new Category
            {
                Id           = Guid.NewGuid(),
                ParentId     = parentId ?? null,
                CategoryName = categoryName.Trim(),
                CategorySlug = categorySlug.Trim().ToLowerInvariant(),
                Description  = description,
                Level        = level,
                IconUrl      = iconUrl,
                ImageUrl     = imageUrl,
                SortOrder    = sortOrder,
                IsActive     = true,
                IsLeaf       = true,
                ProductCount = 0,
                CreatedAt    = DateTime.UtcNow
            };
        }

        public void Update(
            string  categoryName,
            string  categorySlug,
            string? description = null,
            string? iconUrl     = null,
            string? imageUrl    = null,
            int     sortOrder   = 0,
            bool    isActive    = true)
        {
            Guard.AgainstNullOrEmpty(categoryName, nameof(categoryName));
            Guard.AgainstNullOrEmpty(categorySlug, nameof(categorySlug));

            CategoryName = categoryName.Trim();
            CategorySlug = categorySlug.Trim().ToLowerInvariant();
            Description  = description;
            IconUrl      = iconUrl;
            ImageUrl     = imageUrl;
            SortOrder    = sortOrder;
            IsActive     = isActive;
            UpdatedAt    = DateTime.UtcNow;
        }

        public void UpdateParent(Guid? parentId, int level, string path)
        {
            ParentId  = parentId;
            Level     = level;
            Path      = path;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPath(string path)
        {
            Path      = path;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsLeaf()
        {
            IsLeaf    = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnmarkLeaf()
        {
            IsLeaf    = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
