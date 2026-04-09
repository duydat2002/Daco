namespace Daco.Domain.Products.Entities
{
    public class ProductVariant : Entity
    {
        public Guid      ProductId        { get; private set; }
        public string    Sku              { get; private set; }
        public string?   Gtin             { get; private set; }
        public Guid?     Option1Id        { get; private set; }
        public Guid?     Option2Id        { get; private set; }
        public string?   VariantName      { get; private set; }
        public decimal   Price            { get; private set; }
        public decimal?  CompareAtPrice   { get; private set; }
        public int       StockQuantity    { get; private set; }
        public int       ReservedQuantity { get; private set; }
        public decimal?  Weight           { get; private set; }
        public decimal?  Length           { get; private set; }
        public decimal?  Width            { get; private set; }
        public decimal?  Height           { get; private set; }
        public string?   ImageUrl         { get; private set; }
        public int       SoldCount        { get; private set; }
        public bool      IsActive         { get; private set; }
        public DateTime  CreatedAt        { get; private set; }
        public DateTime? UpdatedAt        { get; private set; }

        protected ProductVariant() { }
    }
}
