namespace Daco.Domain.Carts.Entities
{
    public class CartItem : Entity
    {
        public Guid      CartId        { get; private set; }
        public Guid      ShopId        { get; private set; }
        public Guid      ProductId     { get; private set; }
        public Guid?     VariantId     { get; private set; }
        public int       Quantity      { get; private set; }
        public bool      IsSelected    { get; private set; }
        public decimal   PriceSnapshot { get; private set; }
        public DateTime  AddedAt       { get; private set; }
        public DateTime? UpdatedAt     { get; private set; }

        protected CartItem() { } 
    }
}
