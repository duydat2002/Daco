namespace Daco.Domain.Users.Events
{
    public class DefaultAddressChangedEvent : DomainEvent
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
