namespace Daco.Domain.Products.Entities
{
    public class ProductVariantGroup : Entity
    {
        private readonly List<ProductVariantOption> _productVariantOptions = new();

        public Guid     ProductId { get; private set; }
        public string   GroupName { get; private set; }
        public int      GroupType { get; private set; }
        public int      SortOrder { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<ProductVariantOption> ProductVariantOptions => _productVariantOptions.AsReadOnly();

        protected ProductVariantGroup() { }
    }
}
