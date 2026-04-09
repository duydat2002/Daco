namespace Daco.Domain.Messaging.Entities
{
    public class Message : Entity
    {
        public Guid        ConversationId { get; private set; }
        public SenderType  SenderType     { get; private set; }
        public Guid        SenderId       { get; private set; }
        public MessageType MessageType    { get; private set; }
        public string?     Content        { get; private set; }
        public string?     Attachments    { get; private set; } //Array of {type, url, metadata}
        public Guid?       ProductId      { get; private set; }
        public Guid?       OrderId        { get; private set; }
        public Guid?       VoucherId      { get; private set; }
        public bool        IsRead         { get; private set; }
        public DateTime?   ReadAt         { get; private set; }
        public bool        DeletedByBuyer { get; private set; }
        public bool        DeletedByShop  { get; private set; }
        public DateTime    CreatedAt      { get; private set; }

        protected Message() { }
    }
}
