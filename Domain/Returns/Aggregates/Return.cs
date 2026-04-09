namespace Daco.Domain.Returns.Aggregates
{
    public class Return : AggregateRoot
    {
        public Guid         OrderId        { get; private set; }
        public Guid         UserId         { get; private set; }
        public Guid         ShopId         { get; private set; }
        public string       ReturnCode     { get; private set; }
        public ReturnType   ReturnType     { get; private set; }
        public string       ReturnItems    { get; private set; }
        public string       Reason         { get; private set; }
        public string?      Description    { get; private set; }
        public string?      EvidenceImages { get; private set; }
        public decimal      RefundAmount   { get; private set; }
        public ReturnStatus Status         { get; private set; }
        public Guid?        ReviewedBy     { get; private set; }
        public DateTime?    ReviewedAt     { get; private set; }
        public string       RejectReason   { get; private set; }
        public DateTime     CreatedAt      { get; private set; }
        public DateTime?    UpdatedAt      { get; private set; }
        public DateTime?    ApprovedAt     { get; private set; }
        public DateTime?    RefundedAt     { get; private set; }
        public DateTime?    CompletedAt    { get; private set; }

        protected Return() { }
    }
}
