namespace Daco.Domain.Shipping.Entities
{
    public class ShippingService : Entity
    {
        public Guid      ProviderId     { get; private set; }
        public Guid      ShippingTypeId { get; private set; }
        public string    ServiceName    { get; private set; }
        public decimal?  MaxWeight      { get; private set; }
        public decimal?  MaxCodAmount   { get; private set; }
        public bool      IsActive       { get; private set; }
        public DateTime  CreatedAt      { get; private set; }
        public DateTime? UpdatedAt      { get; private set; }

        protected ShippingService() { }
    }
}
