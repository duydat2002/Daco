namespace Daco.Domain.Products.Entities
{
    public class ProductDynamicAttribute : Entity
    {
        public Guid      ProductId    { get; private set; }
        public Guid      AttributeId  { get; private set; }
        public string?   ValueText    { get; private set; }
        public decimal?  ValueNumber  { get; private set; }
        public DateOnly? ValueDate    { get; private set; }
        public bool?     ValueBoolean { get; private set; }
        public DateTime  CreatedAt    { get; private set; }
        public DateTime? UpdatedAt    { get; private set; }

        protected ProductDynamicAttribute() { }
    }
}
