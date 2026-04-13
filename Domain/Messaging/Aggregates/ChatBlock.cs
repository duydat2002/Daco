namespace Daco.Domain.Messaging.Aggregates
{
    public class ChatBlock : AggregateRoot
    {
        public Guid          BlockerUserId { get; private set; }
        public Guid          BlockedUserId { get; private set; }
        public ChatBlockType BlockType     { get; private set; }
        public string        Reason        { get; private set; }
        public DateTime      CreatedAt     { get; private set; }

        protected ChatBlock() { }
    }
}
