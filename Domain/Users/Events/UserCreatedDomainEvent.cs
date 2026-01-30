namespace Daco.Domain.Users.Events
{
    public record UserCreatedDomainEvent(Guid Id) : DomainEvent
    {
    }
}
