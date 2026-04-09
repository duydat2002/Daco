namespace Daco.Domain.Shipping.Entities
{
    public class ShippingType : Entity
    {
        public string    Code             { get; private set; }
        public string    Name             { get; private set; }
        public string?   Description      { get; private set; }
        public int?      EstimatedDaysMin { get; private set; }
        public int?      EstimatedDaysMax { get; private set; }
        public string    IconUrl          { get; private set; }
        public int       SortOrder        { get; private set; }
        public bool      IsActive         { get; private set; }
        public DateTime  CreatedAt        { get; private set; }
        public DateTime? UpdatedAt        { get; private set; }

        protected ShippingType() { }
    }
}
