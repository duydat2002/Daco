namespace Daco.Domain.Administration.Entities
{
    public class AdminRoleAssignment : Entity
    {
        public Guid      AdminId    { get; private set; }
        public Guid      RoleId     { get; private set; }
        public Guid      AssignedBy { get; private set; }
        public DateTime  AssignedAt { get; private set; }
        public DateTime? ExpiresAt  { get; private set; }
        public bool      IsActive   { get; private set; }

        protected AdminRoleAssignment() { }

        public static AdminRoleAssignment Create(
            Guid adminId,
            Guid roleId,
            Guid assignedBy,
            DateTime? expiresAt = null)
        {
            return new AdminRoleAssignment
            {
                Id = Guid.NewGuid(),
                AdminId = adminId,
                RoleId = roleId,
                AssignedBy = assignedBy,
                AssignedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                IsActive = true
            };
        }

        public void Revoke()
        {
            IsActive = false;
        }

        public bool IsExpired() =>
            ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    }
}
