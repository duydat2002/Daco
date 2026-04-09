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
    }
}
