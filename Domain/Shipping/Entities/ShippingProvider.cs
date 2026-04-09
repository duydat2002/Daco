namespace Daco.Domain.Shipping.Entities
{
    public class ShippingProvider : Entity
    {
        private readonly List<ShippingService> _shippingServices = new();

        public string    ProviderCode    { get; private set; }
        public string    ProviderName    { get; private set; }
        public string?   LogoUrl         { get; private set; }
        public bool      SupportCod      { get; private set; }
        public bool      SupportTracking { get; private set; }
        public bool      IsActive        { get; private set; }
        public DateTime  CreatedAt       { get; private set; }
        public DateTime? UpdatedAt       { get; private set; }

        public IReadOnlyCollection<ShippingService> ShippingServices => _shippingServices.AsReadOnly();

        protected ShippingProvider() { }
    }
}
