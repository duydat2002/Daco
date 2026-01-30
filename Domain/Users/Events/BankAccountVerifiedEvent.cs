namespace Daco.Domain.Users.Events
{
    public record BankAccountVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid BankAccountId { get; init; }

        public BankAccountVerifiedEvent(Guid userId, Guid bankAccountId)
        {
            UserId = userId;
            BankAccountId = bankAccountId;
        }
    }
}
