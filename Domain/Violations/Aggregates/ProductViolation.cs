namespace Daco.Domain.Violations.Aggregates
{
    public class ProductViolation : AggregateRoot
    {
        private readonly List<ViolationActionsLog> _violationActionsLogs = new();

        public Guid            ProductId       { get; private set; }
        public Guid            ShopId          { get; private set; }
        public Guid            ViolationTypeId { get; private set; }
        public ViolationSource Source          { get; private set; }
        public Guid?           ReporterId      { get; private set; }
        public string          Description     { get; private set; }
        public string?         EvidenceUrls    { get; private set; }
        public ViolationStatus Status          { get; private set; }
        public Guid?           ReviewedBy      { get; private set; }
        public DateTime?       ReviewedAt      { get; private set; }
        public string?         ReviewNotes     { get; private set; }
        public string?         ActionsTaken    { get; private set; }
        public string?         ProductSnapshot { get; private set; }
        public Guid?           ResolvedBy      { get; private set; }
        public DateTime?       ResolvedAt      { get; private set; }
        public string?         ResolutionNotes { get; private set; }
        public DateTime        CreatedAt       { get; private set; }
        public DateTime?       UpdatedAt       { get; private set; }

        public IReadOnlyCollection<ViolationActionsLog> ViolationActionsLogs => _violationActionsLogs.AsReadOnly();

        protected ProductViolation() { }
    }
}
