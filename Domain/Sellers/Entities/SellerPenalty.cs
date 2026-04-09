namespace Daco.Domain.Sellers.Entities
{
    public class SellerPenalty : Entity
    {
        public Guid          SellerId          { get; private set; }
        public Guid?         ViolationId       { get; private set; }
        public PenaltyType   PenaltyType       { get; private set; }
        public string        Reason            { get; private set; }
        public int           PenaltyPoints     { get; private set; }
        public decimal?      FineAmount        { get; private set; }
        public DateTime?     SuspendFrom       { get; private set; }
        public DateTime?     SuspendTo         { get; private set; }
        public Guid          IssuedBy          { get; private set; }
        public string?       AppealNote        { get; private set; }
        public DateTime?     AppealSubmittedAt { get; private set; }
        public AppealStatus? AppealStatus      { get; private set; }
        public Guid?         AppealReviewedBy  { get; private set; }
        public DateTime?     AppealReviewedAt  { get; private set; }
        public DateTime      CreatedAt         { get; private set; }

        protected SellerPenalty() { }
    }
}
