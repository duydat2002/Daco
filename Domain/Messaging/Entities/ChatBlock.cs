namespace Daco.Domain.Messaging.Entities
{
    public class ChatBlock : Entity
    {
        public Guid     BlockerUserId { get; private set; }
        public Guid     BlockedUserId { get; private set; }
        public string   BlockType     { get; private set; }
        public string   Reason        { get; private set; }
        public DateTime CreatedAt     { get; private set; }

        protected ChatBlock() { }
    }
}
