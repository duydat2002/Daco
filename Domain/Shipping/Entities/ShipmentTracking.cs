namespace Daco.Domain.Shipping.Entities
{
    public class ShipmentTracking : Entity
    {
        public Guid              ShipmentId        { get; private set; }
        public TrackingEventType EventType         { get; private set; }
        public string?           Exception         { get; private set; }
        public string            EventCode         { get; private set; }
        public string            StatusDescription { get; private set; }
        public string?           Location          { get; private set; }
        public string?           Address           { get; private set; }
        public decimal?          Latitude          { get; private set; }
        public decimal?          Longitude         { get; private set; }
        public DateTime          EventTime         { get; private set; }
        public string?           HandledBy         { get; private set; }
        public string?           ContactPhone      { get; private set; }
        public string?           SignatureUrl      { get; private set; }
        public string?           PhotoUrl          { get; private set; }
        public string?           Notes             { get; private set; }
        public string?           ProviderData      { get; private set; }
        public DateTime          CreatedAt         { get; private set; }

        protected ShipmentTracking() { }
    }
}
