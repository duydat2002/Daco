using Daco.Domain.Products.Entities;

namespace Daco.Domain.Brands.Aggregates
{
    public class Brand : AggregateRoot
    {
        private readonly List<BrandCategory> _brandCategories = new();

        public string        BrandName    { get; private set; }
        public string        BrandSlug    { get; private set; }
        public string?       Description  { get; private set; }
        public string?       WebsiteUrl   { get; private set; }
        public string?       LogoUrl      { get; private set; }
        public List<string>  SampleImages { get; private set; } = [];
        public bool          IsActive     { get; private set; }
        public DateTime      CreatedAt    { get; private set; }
        public DateTime?     UpdatedAt    { get; private set; }
        public DateTime?     DeletedAt    { get; private set; }

        public IReadOnlyCollection<BrandCategory> BrandCategories => _brandCategories.AsReadOnly();

        protected Brand() { }

        public static Brand Create(
            string brandName,
            string brandSlug,
            string? description = null,
            string? websiteUrl = null)
        {
            Guard.AgainstNullOrEmpty(brandName, nameof(brandName));
            Guard.AgainstNullOrEmpty(brandSlug, nameof(brandSlug));

            return new Brand
            {
                Id = Guid.NewGuid(),
                BrandName = brandName.Trim(),
                BrandSlug = brandSlug.Trim().ToLowerInvariant(),
                Description = description,
                WebsiteUrl = websiteUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(
            string brandName,
            string brandSlug,
            string? description = null,
            string? websiteUrl = null,
            string? logoUrl = null)
        {
            Guard.AgainstNullOrEmpty(brandName, nameof(brandName));
            Guard.AgainstNullOrEmpty(brandSlug, nameof(brandSlug));

            BrandName = brandName.Trim();
            BrandSlug = brandSlug.Trim().ToLowerInvariant();
            Description = description;
            WebsiteUrl = websiteUrl;
            LogoUrl = logoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsActive = true;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignCategory(Guid categoryId)
        {
            Guard.Against(categoryId == Guid.Empty, "CategoryId is required");
            Guard.Against(
                _brandCategories.Any(bc => bc.CategoryId == categoryId),
                "Category is already assigned to this brand");

            var brandCategory = BrandCategory.Create(Id, categoryId);
            _brandCategories.Add(brandCategory);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnassignCategory(Guid categoryId)
        {
            var brandCategory = _brandCategories.FirstOrDefault(bc => bc.CategoryId == categoryId);
            Guard.Against(brandCategory is null, "Category is not assigned to this brand");

            _brandCategories.Remove(brandCategory!);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddLogo(string logoUrl)
        {
            Guard.AgainstNull(logoUrl, nameof(logoUrl));

            LogoUrl = logoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddSample(string sampleUrl)
        {
            Guard.AgainstNull(sampleUrl, nameof(sampleUrl));
            Guard.Against(
                SampleImages is { Count: > 10 },
                "Sample images must not exceed 10 items");
            Guard.Against(
                SampleImages is { Count: < 1 },
                "Sample images must contain at least one image");

            SampleImages.Add(sampleUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteSample(string sampleUrl)
        {
            Guard.AgainstNull(sampleUrl, nameof(sampleUrl));
            Guard.Against(
                SampleImages is { Count: > 10 },
                "Sample images must not exceed 10 items");
            Guard.Against(
                SampleImages is { Count: <= 1 },
                "Sample images must contain at least one image");

            SampleImages.Remove(sampleUrl);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
