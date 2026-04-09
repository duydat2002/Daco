namespace Daco.Domain.Reviews.Aggregates
{
    public class Review : AggregateRoot
    {
        private readonly List<ReviewReaction> _reviewReactions = new();

        public Guid         OrderId            { get; private set; }
        public Guid         OrderItemId        { get; private set; }
        public Guid         UserId             { get; private set; }
        public Guid         ShopId             { get; private set; }
        public Guid         ProductId          { get; private set; }
        public Guid?        VariantId          { get; private set; }
        public int          Rating             { get; private set; }
        public string?      Comment            { get; private set; }
        public string?      Images             { get; private set; }
        public string?      Videos             { get; private set; }
        public bool         IsVerifiedPurchase { get; private set; }
        public int          HelpfulCount       { get; private set; }
        public string?      SellerReply        { get; private set; }
        public DateTime?    SellerRepliedAt    { get; private set; }
        public ReviewStatus Status             { get; private set; }
        public DateTime     CreatedAt          { get; private set; }
        public DateTime?    UpdatedAt          { get; private set; }
        public IReadOnlyCollection<ReviewReaction> ReviewReactions => _reviewReactions.AsReadOnly();

        protected Review() { }
    }
}
