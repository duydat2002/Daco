namespace Daco.Domain.Administration.Events
{
    public class AdminCreatedEvent : DomainEvent
    {
        public Guid AdminId   { get; init; }
        public Guid UserId    { get; init; }
        public Guid CreatedBy { get; init; }

        public AdminCreatedEvent(Guid adminId, Guid userId, Guid createdBy)
        {
            AdminId   = adminId;
            UserId    = userId;
            CreatedBy = createdBy;
        }
    }

    public class AdminRoleAssignedEvent : DomainEvent
    {
        public Guid   AdminId    { get; init; }
        public Guid   UserId     { get; init; }
        public string RoleCode   { get; init; }
        public Guid   AssignedBy { get; init; }

        public AdminRoleAssignedEvent(Guid adminId, Guid userId, string roleCode, Guid assignedBy)
        {
            AdminId    = adminId;
            UserId     = userId;
            RoleCode   = roleCode;
            AssignedBy = assignedBy;
        }
    }

    public class AdminDeactivatedEvent : DomainEvent
    {
        public Guid AdminId { get; init; }
        public Guid UserId  { get; init; }

        public AdminDeactivatedEvent(Guid adminId, Guid userId)
        {
            AdminId = adminId;
            UserId  = userId;
        }
    }

    public class AdminSuspendedEvent : DomainEvent
    {
        public Guid   AdminId { get; init; }
        public Guid   UserId  { get; init; }
        public string Reason  { get; init; }

        public AdminSuspendedEvent(Guid adminId, Guid userId, string reason)
        {
            AdminId = adminId;
            UserId  = userId;
            Reason  = reason;
        }
    }
}
