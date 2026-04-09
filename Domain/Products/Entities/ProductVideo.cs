namespace Daco.Domain.Products.Entities
{
    public class ProductVideo : Entity
    {
        public Guid     ProductId    { get; private set; }
        public string   VideoUrl     { get; private set; }
        public string?  ThumbnailUrl { get; private set; }
        public int      Duration     { get; private set; }
        public int      SortOrder    { get; private set; }
        public DateTime CreatedAt    { get; private set; }

        protected ProductVideo() { }
    }
}
