namespace Daco.Domain.Products.Entities
{
    public class ProductVideo : Entity
    {
        public Guid     ProductId    { get; private set; }
        public string   VideoUrl     { get; private set; }
        public string?  ThumbnailUrl { get; private set; }
        public int?     Duration     { get; private set; }
        public int      SortOrder    { get; private set; }
        public DateTime CreatedAt    { get; private set; }

        protected ProductVideo() { }

        public static ProductVideo Create(
            Guid productId,
            string videoUrl,
            string thumbnailUrl)
        {
            Guard.Against(productId == Guid.Empty, "ProductId is required");
            Guard.AgainstNullOrEmpty(videoUrl, nameof(videoUrl));
            Guard.AgainstNullOrEmpty(thumbnailUrl, nameof(thumbnailUrl));

            return new ProductVideo
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                VideoUrl = videoUrl,
                ThumbnailUrl = thumbnailUrl,
                Duration = 0,
                SortOrder = 0,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
