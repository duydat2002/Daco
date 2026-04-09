namespace Daco.Domain.Brands.Aggregates
{
    public class Brand : AggregateRoot
    {
        private readonly List<BrandCategory> _brandCategories = new();

        public string    BrandName    { get; private set; }
        public string    BrandSlug    { get; private set; }
        public string?   Description  { get; private set; }
        public string?   WebsiteUrl   { get; private set; }
        public string?   LogoUrl      { get; private set; }
        public string[]? SampleImages { get; private set; }
        public bool      IsActive     { get; private set; }
        public DateTime  CreatedAt    { get; private set; }
        public DateTime? UpdatedAt    { get; private set; }

        public IReadOnlyCollection<BrandCategory> BrandCategories => _brandCategories.AsReadOnly();


        protected Brand() { }
    }
}
