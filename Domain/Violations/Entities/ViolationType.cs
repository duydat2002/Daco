namespace Daco.Domain.Violations.Entities
{
    public class ViolationType : Entity
    {
        public string    Code            { get; private set; }
        public string    Name            { get; private set; }
        public string?   Description     { get; private set; }
        public string    Severity        { get; private set; }
        public bool      AutoHideProduct { get; private set; }
        public bool      AutoSuspendShop { get; private set; }
        public int       PenaltyPoints   { get; private set; }
        public bool      IsActive        { get; private set; }
        public DateTime  CreatedAt       { get; private set; }
        public DateTime? UpdatedAt       { get; private set; }

        protected ViolationType() { }
    }
}
