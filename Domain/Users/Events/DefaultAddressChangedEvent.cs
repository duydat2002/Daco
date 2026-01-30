namespace Daco.Domain.Users.Events
{
    public record DefaultAddressChangedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid AddressId { get; init; }

        public DefaultAddressChangedEvent(Guid userId, Guid addressId)
        {
            UserId = userId;
            AddressId = addressId;
        }
    }
}
