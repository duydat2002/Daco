namespace Daco.Domain.Notifications.Entities
{
    public class NotificationLog : Entity
    {
        public Guid                NotificationId       { get; private set; }
        public NotificationChannel Channel              { get; private set; }
        public string?             RecipientEmail       { get; private set; }
        public string?             RecipientPhone       { get; private set; }
        public string?             RecipientDeviceToken { get; private set; }
        public DeliveryStatus      Status               { get; private set; }
        public string?             GatewayProvider      { get; private set; }
        public string?             GatewayMessageId     { get; private set; }
        public string?             GatewayResponse      { get; private set; }
        public DateTime?           SentAt               { get; private set; }
        public DateTime?           DeliveredAt          { get; private set; }
        public DateTime?           FailedAt             { get; private set; }
        public string?             ErrorMessage         { get; private set; }
        public DateTime            CreatedAt            { get; private set; }

        protected NotificationLog() { }
    }
}
