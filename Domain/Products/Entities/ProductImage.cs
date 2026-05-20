namespace Daco.Domain.Products.Entities
{
    public class ProductImage : Entity
    {
        public Guid     ProductId { get; private set; }
        public string   ImageUrl  { get; private set; }
        public string?  AltText   { get; private set; }
        public bool     IsCover   { get; private set; }
        public int      SortOrder { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected ProductImage() { }

        public static ProductImage Create(
            Guid productId,
            string imageUrl,
            int sortOrder,
            string? altText = null)
        {
            Guard.Against(productId == Guid.Empty, "ProductId is required");
            Guard.AgainstNullOrEmpty(imageUrl, nameof(imageUrl));
            Guard.AgainstNegative(sortOrder, nameof(sortOrder));

            return new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ImageUrl = imageUrl,
                AltText = altText,
                IsCover = sortOrder == 0,
                SortOrder = sortOrder,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void UpdateSortOrder(int sortOrder)
        {
            Guard.AgainstNegative(sortOrder, nameof(sortOrder));
            SortOrder = sortOrder;
            IsCover = sortOrder == 0;
        }
    }
}
