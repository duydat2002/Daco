using Domain.Common;

namespace Domain.Users.Events
{
    public record UserCreatedDomainEvent(Guid Id) : DomainEvent
    {
    }
}
