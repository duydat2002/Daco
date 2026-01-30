namespace Daco.Domain.Users.Events
{
    public record BankAccountRemovedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid BankAccountId { get; init; }

        public BankAccountRemovedEvent(Guid userId, Guid bankAccountId)
        {
            UserId = userId;
            BankAccountId = bankAccountId;
        }
    }
}
