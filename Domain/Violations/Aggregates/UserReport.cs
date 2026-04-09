namespace Daco.Domain.Violations.Aggregates
{
    public class UserReport : AggregateRoot
    {
        public Guid         ReporterId   { get; private set; }
        public string       EntityType   { get; private set; }
        public Guid         EntityId     { get; private set; }
        public string       Reason       { get; private set; }
        public string?      Description  { get; private set; }
        public string?      EvidenceUrls { get; private set; }
        public ReportStatus Status       { get; private set; }
        public Guid?        AssignedTo   { get; private set; }
        public DateTime?    ReviewedAt   { get; private set; }
        public string?      ReviewNotes  { get; private set; }
        public string?      ActionTaken  { get; private set; }
        public DateTime?    ResolvedAt   { get; private set; }
        public DateTime     CreatedAt    { get; private set; }
        public DateTime?    UpdatedAt    { get; private set; }

        protected UserReport() { }
    }
}
