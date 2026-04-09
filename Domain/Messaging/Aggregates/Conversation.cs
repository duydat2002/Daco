using Daco.Domain.Users.Entities;

namespace Daco.Domain.Messaging.Aggregates
{
    public class Conversation : AggregateRoot
    {
        private readonly List<Message> _messages = new();
        private readonly List<ChatBlock> _chatBlocks = new();

        public Guid      BuyerId            { get; private set; }
        public Guid      ShopId             { get; private set; }
        public Guid?     LastMessageId      { get; private set; }
        public DateTime? LastMessageAt      { get; private set; }
        public string?   LastMessagePreview { get; private set; }
        public int       UnreadCountBuyer   { get; private set; }
        public int       UnreadCountShop    { get; private set; }
        public bool      IsBlocked          { get; private set; }
        public DateTime  CreatedAt          { get; private set; }
        public DateTime? UpdatedAt          { get; private set; }

        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();
        public IReadOnlyCollection<ChatBlock> ChatBlocks => _chatBlocks.AsReadOnly();

        protected Conversation() { }
    }
}
