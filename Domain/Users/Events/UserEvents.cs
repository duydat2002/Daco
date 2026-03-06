namespace Daco.Domain.Users.Events
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Identifier { get; init; }
        public string ProviderType { get; init; }

        public UserRegisteredEvent(Guid userId, string identifier, string providerType)
        {
            UserId = userId;
            Identifier = identifier;
            ProviderType = providerType;
        }
    }

    public class EmailVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; }

        public EmailVerifiedEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }

    public class PhoneVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Phone { get; init; }

        public PhoneVerifiedEvent(Guid userId, string phone)
        {
            UserId = userId;
            Phone = phone;
        }
    }

    public class PasswordChangedEvent : DomainEvent
    {
        public Guid UserId { get; init; }

        public PasswordChangedEvent(Guid userId)
        {
            UserId = userId;
        }
    }

    public class AddressAddedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid AddressId { get; init; }

        public AddressAddedEvent(Guid userId, Guid addressId)
        {
            UserId = userId;
            AddressId = addressId;
        }
    }

    public class UserSuspendedEvent : DomainEvent
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
