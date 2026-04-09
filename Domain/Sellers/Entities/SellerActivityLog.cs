namespace Daco.Domain.Sellers.Entities
{
    public class SellerActivityLog : Entity
    {
        public Guid     SellerId            { get; private set; }
        public Guid     UserId              { get; private set; }
        public string   ActivityType        { get; private set; }
        public string?  ActivityDescription { get; private set; }
        public string?  TargetType          { get; private set; } //'product', 'order', 'settings'
        public Guid?    TargetId            { get; private set; }
        public string?  OldValue            { get; private set; }
        public string?  NewValue            { get; private set; }
        public string?  IpAddress           { get; private set; }
        public string?  UserAgent           { get; private set; }
        public string?  DeviceType          { get; private set; }
        public DateTime CreatedAt           { get; private set; }

        protected SellerActivityLog() { }
    }
}
