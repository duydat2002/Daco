namespace Daco.Domain.Users.Events
{
    public record UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Identifier { get; init; }
        public ProviderType ProviderType { get; init; }

        public UserRegisteredEvent(Guid userId, string identifier, ProviderType providerType)
        {
            UserId = userId;
            Identifier = identifier;
            ProviderType = providerType;
        }
    }

    public record EmailVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; }

        public EmailVerifiedEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }

    public record PhoneVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Phone { get; init; }

        public PhoneVerifiedEvent(Guid userId, string phone)
        {
            UserId = userId;
            Phone = phone;
        }
    }

    public record PasswordChangedEvent : DomainEvent
    {
        public Guid UserId { get; init; }

        public PasswordChangedEvent(Guid userId)
        {
            UserId = userId;
        }
    }

    public record AddressAddedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid AddressId { get; init; }

        public AddressAddedEvent(Guid userId, Guid addressId)
        {
            UserId = userId;
            AddressId = addressId;
        }
    }

    public record UserSuspendedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Reason { get; init; }

        public UserSuspendedEvent(Guid userId, string reason)
        {
            UserId = userId;
            Reason = reason;
        }
    }
}
