namespace Daco.Domain.Sellers.Entities
{
    public class SellerPenaltyScore : Entity
    {
        public Guid      SellerId        { get; private set; }
        public int       TotalPoints     { get; private set; }
        public int       WarningCount    { get; private set; }
        public int       ViolationCount  { get; private set; }
        public DateTime? LastViolationAt { get; private set; }
        public RiskLevel RiskLevel       { get; private set; }
        public DateTime  UpdatedAt       { get; private set; }

        protected SellerPenaltyScore() { }
    }
}
