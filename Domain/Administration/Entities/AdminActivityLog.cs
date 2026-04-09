namespace Daco.Domain.Administration.Entities
{
    public class AdminActivityLog : Entity
    {
        public Guid     AdminId           { get; private set; }
        public string   ActionType        { get; private set; } //'user.ban', 'shop.approve', 'product.delete'
        public string?  ActionDescription { get; private set; }
        public string?  TargetType        { get; private set; } //'user', 'shop', 'product', 'order'
        public Guid?    TargetId          { get; private set; }
        public string   OldValue          { get; private set; }
        public string   NewValue          { get; private set; }
        public string?  IpAddress         { get; private set; }
        public string?  UserAgent         { get; private set; }
        public DateTime CreatedAt         { get; private set; }

        protected AdminActivityLog() { }
    }
}
