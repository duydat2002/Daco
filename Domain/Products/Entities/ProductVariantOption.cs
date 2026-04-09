namespace Daco.Domain.Products.Entities
{
    public class ProductVariantOption : Entity
    {
        public Guid     VariantGroupId { get; private set; }
        public string   OptionName     { get; private set; }
        public string?  OptionValue    { get; private set; }
        public string?  ImageUrl       { get; private set; }
        public string?  ColorHex       { get; private set; }
        public int      SortOrder      { get; private set; }
        public bool     IsActive       { get; private set; }
        public DateTime CreatedAt      { get; private set; }

        protected ProductVariantOption() { }
    }
}
