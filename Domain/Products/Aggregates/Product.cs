namespace Daco.Domain.Products.Aggregates
{
    public class Product : AggregateRoot
    {
        private readonly List<ProductImage> _productImages = new();
        private readonly List<ProductVideo> _productVideos = new();
        private readonly List<ProductDynamicAttribute> _productDynamicAttributes = new();
        private readonly List<ProductVariantGroup> _productVariantGroups = new();
        private readonly List<ProductVariant> _productVariants = new();
        
        public Guid          ShopId               { get; private set; }
        public Guid          CategoryId           { get; private set; }
        public string        ProductName          { get; private set; }
        public string        ProductSlug          { get; private set; }
        public string?       Description          { get; private set; }
        public Guid          BrandId              { get; private set; }
        public decimal?      BasePrice            { get; private set; }
        public decimal?      CompareAtPrice       { get; private set; }
        public int           StockQuantity        { get; private set; }
        public string?       Sku                  { get; private set; }
        public string?       Gtin                 { get; private set; }
        public ProductStatus Status               { get; private set; }
        public bool          HasVariants          { get; private set; }
        public bool          IsPreOrder           { get; private set; }
        public int           PreOrderLeadTime     { get; private set; }
        public decimal       Weight               { get; private set; }
        public decimal       Length               { get; private set; }
        public decimal       Width                { get; private set; }
        public decimal       Height               { get; private set; }
        public string[]?     EnabledShippingTypes { get; private set; }
        public int           ViewCount            { get; private set; }
        public int           SoldCount            { get; private set; }
        public int           WishlistCount        { get; private set; }
        public decimal       RatingAverage        { get; private set; }
        public int           ReviewCount          { get; private set; }
        public string?       MetaTitle            { get; private set; }
        public string?       MetaDescription      { get; private set; }
        public string?       MetaKeywords         { get; private set; }
        public DateTime      CreatedAt            { get; private set; }
        public DateTime?     UpdatedAt            { get; private set; }
        public DateTime?     PublishedAt          { get; private set; }
        public DateTime?     DeletedAt            { get; private set; }

        public IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();
        public IReadOnlyCollection<ProductVideo> ProductVideos => _productVideos.AsReadOnly();
        public IReadOnlyCollection<ProductDynamicAttribute> ProductDynamicAttributes => _productDynamicAttributes.AsReadOnly();
        public IReadOnlyCollection<ProductVariantGroup> ProductVariantGroups => _productVariantGroups.AsReadOnly();
        public IReadOnlyCollection<ProductVariant> ProductVariants => _productVariants.AsReadOnly();

        protected Product() { }

        public void Approve(Guid approvedBy)
        {
            Guard.Against(Status != ProductStatus.Pending, "Only pending products can be approved");

            Status      = ProductStatus.Active;
            PublishedAt = DateTime.UtcNow;
            UpdatedAt   = DateTime.UtcNow;

            AddDomainEvent(new ProductApprovedEvent(Id, ShopId, approvedBy));
        }

        public void Suspend(Guid suspendedBy, string reason)
        {
            Guard.Against(Status != ProductStatus.Pending, "Only pending products can be approved");

            Status = ProductStatus.Active;
            PublishedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new ProductSuspendedEvent(Id, ShopId, suspendedBy, reason));
        }

        public void UnSuspend(Guid unsuspendedBy, string reason)
        {
            Guard.Against(Status != ProductStatus.Suspended, "Only suspend products can be unsuspend");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = ProductStatus.Active;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new ProductUnSuspendedEvent(Id, ShopId, unsuspendedBy, reason));
        }

        public void Remove(Guid removedBy, string reason)
        {
            Guard.Against(Status == ProductStatus.Deleted, "Product already deleted");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = ProductStatus.Deleted;
            DeletedAt = DateTime.UtcNow;

            AddDomainEvent(new ProductRemovedEvent(Id, ShopId, removedBy, reason));
        }
    }
}
