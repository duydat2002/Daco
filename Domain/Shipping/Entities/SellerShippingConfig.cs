namespace Daco.Domain.Shipping.Entities
{
    public class SellerShippingConfig : Entity
    {
        public Guid      SellerId         { get; private set; }
        public bool      EnableExpress    { get; private set; }
        public bool      EnableSameDay    { get; private set; }
        public bool      EnableFast       { get; private set; }
        public bool      EnableEconomy    { get; private set; }
        public bool      EnableBulky      { get; private set; }
        public bool      EnableExpressCod { get; private set; }
        public bool      EnableSameDayCod { get; private set; }
        public bool      EnableFastCod    { get; private set; }
        public bool      EnableEconomyCod { get; private set; }
        public bool      EnableBulkyCod   { get; private set; }
        public DateTime  CreatedAt        { get; private set; }
        public DateTime? UpdatedAt        { get; private set; }

        protected SellerShippingConfig() { }
    }
}
