namespace Daco.Domain.Violations.Entities
{
    public class ViolationActionsLog : Entity
    {
        public Guid     ViolationId   { get; private set; }
        public string   ActionType    { get; private set; }
        public string?  ActionDetails { get; private set; }
        public Guid     PerformedBy   { get; private set; }
        public DateTime PerformedAt   { get; private set; }

        protected ViolationActionsLog() { }
    }
}
