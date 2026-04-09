namespace Daco.Domain.Shipping.Aggregates
{
    public class Shipment : AggregateRoot
    {
        private readonly List<ShipmentTracking> _shipmentTrackings = new();

        public Guid           OrderId             { get; private set; }
        public Guid           ShopId              { get; private set; }
        public Guid           ServiceId           { get; private set; }
        public Guid           ProviderId          { get; private set; }
        public string         TrackingNumber      { get; private set; }
        public decimal        Weight              { get; private set; }
        public decimal        ShippingFee         { get; private set; }
        public decimal        CodFee              { get; private set; }
        public decimal        TotalFee            { get; private set; }
        public decimal        CodAmount           { get; private set; }
        public ShipmentStatus Status              { get; private set; }
        public DateTime?      EstimatedDeliveryAt { get; private set; }
        public DateTime?      DeliveredAt         { get; private set; }
        public DateTime       CreatedAt           { get; private set; }
        public DateTime?      UpdatedAt           { get; private set; }

        public IReadOnlyCollection<ShipmentTracking> ShipmentTrackings => _shipmentTrackings.AsReadOnly();

        protected Shipment() { }
    }
}
