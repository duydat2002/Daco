namespace Daco.Domain.Notifications.Entites
{
    public class NotificationTemplate : Entity
    {
        public string    TemplateKey          { get; private set; }
        public string    TitleTemplate        { get; private set; }
        public string    MessageTemplate      { get; private set; }
        public string?   EmailSubjectTemplate { get; private set; }
        public string?   EmailBodyTemplate    { get; private set; }
        public string?   SmsTemplate          { get; private set; }
        public string?   PushTitleTemplate    { get; private set; }
        public string?   PushBodyTemplate     { get; private set; }
        public string?   AvailableVariables   { get; private set; }
        public bool      IsActive             { get; private set; }
        public DateTime  CreatedAt            { get; private set; }
        public DateTime? UpdatedAt            { get; private set; }

        protected NotificationTemplate() { }
    }
}
