namespace Daco.Domain.Users.Events
{
    public record AddressUpdatedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid AddressId { get; init; }

        public AddressUpdatedEvent(Guid userId, Guid addressId)
        {
            UserId = userId;
            AddressId = addressId;
        }
    }
}
