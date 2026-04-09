namespace Daco.Domain.Notifications.Aggregates
{
    public class Notification : AggregateRoot
    {
        public Guid?                UserId           { get; private set; }
        public Guid?                ShopId           { get; private set; }
        public string               NotificationType { get; private set; }
        public NotificationPriority Priority         { get; private set; }
        public string               Title            { get; private set; }
        public string               Message          { get; private set; }
        public string?              ActionUrl        { get; private set; }
        public string?              ActionLabel      { get; private set; }
        public string?              ReferenceType    { get; private set; }
        public string?              ReferenceId      { get; private set; }
        public string?              ImageUrl         { get; private set; }
        public string?              Icon             { get; private set; }
        public bool                 IsRead           { get; private set; }
        public DateTime?            ReadAt           { get; private set; }
        public string[]             SentVia          { get; private set; } // ['push', 'email', 'sms']
        public DateTime?            ExpiresAt        { get; private set; }
        public DateTime             CreatedAt        { get; private set; }

        protected Notification() { }    
    }
}
