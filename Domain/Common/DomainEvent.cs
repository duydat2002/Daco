namespace Daco.Domain.Common
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; init; }
        public DateTime OccurredOn { get; init; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
}
