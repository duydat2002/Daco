namespace Daco.Domain.Administration.Aggregates
{
    public class AdminUser : AggregateRoot
    {
        private readonly List<AdminRoleAssignment> _roleAssignments = new();
        private readonly List<AdminCustomPermission> _customPermissions = new();

        public Guid        UserId       { get; private set; }
        public string?     EmployeeCode { get; private set; }
        public string?     Department   { get; private set; }
        public string?     Position     { get; private set; }
        public string?     WorkEmail    { get; private set; }
        public string?     WorkPhone    { get; private set; }
        public AdminStatus Status       { get; private set; }
        public string?     Notes        { get; private set; }
        public Guid?       AssignedBy   { get; private set; }
        public DateTime    CreatedAt    { get; private set; }
        public DateTime?   UpdatedAt    { get; private set; }

        public IReadOnlyCollection<AdminRoleAssignment> RoleAssignments => _roleAssignments.AsReadOnly();
        public IReadOnlyCollection<AdminCustomPermission> CustomPermissions => _customPermissions.AsReadOnly();

        protected AdminUser() { }

        public static AdminUser Create(
            Guid userId,
            Guid assignedBy,
            string employeeCode,
            string? department = null,
            string? position = null,
            string? workEmail = null,
            string? workPhone = null,
            string? notes = null)
        {
            Guard.Against(userId == Guid.Empty, "UserId is required");
            Guard.Against(assignedBy == Guid.Empty, "AssignedBy is required");
            Guard.AgainstNullOrEmpty(employeeCode, nameof(employeeCode));

            var admin = new AdminUser
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EmployeeCode = employeeCode,
                Department = department,
                Position = position,
                WorkEmail = workEmail,
                WorkPhone = workPhone,
                Status = AdminStatus.Active,
                Notes = notes,
                AssignedBy = assignedBy,
                CreatedAt = DateTime.UtcNow
            };

            admin.AddDomainEvent(new AdminCreatedEvent(admin.Id, userId, assignedBy));
            return admin;
        }

        #region Role management
        public void AssignRole(Guid roleId, Guid assignedBy, DateTime? expiresAt = null)
        {
            Guard.Against(Status != AdminStatus.Active, "Cannot assign role to inactive admin");

            var existing = _roleAssignments
                .FirstOrDefault(r => r.RoleId == roleId && r.IsActive);

            Guard.Against(existing != null, "Role is already assigned");

            var assignment = AdminRoleAssignment.Create(Id, roleId, assignedBy, expiresAt);
            _roleAssignments.Add(assignment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RevokeRole(Guid roleId)
        {
            var assignment = _roleAssignments
                .FirstOrDefault(r => r.RoleId == roleId && r.IsActive);

            Guard.Against(assignment == null, "Role is not assigned");

            assignment!.Revoke();
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddCustomPermission(
            Guid permissionId,
            string permissionCode,
            bool isGranted,
            Guid grantedBy,
            string? reason = null)
        {
            var existing = _customPermissions
                .FirstOrDefault(p => p.PermissionId == permissionId);

            if (existing != null)
                _customPermissions.Remove(existing);

            var custom = isGranted
                ? AdminCustomPermission.Grant(Id, permissionId, permissionCode, grantedBy, reason)
                : AdminCustomPermission.Revoke(Id, permissionId, permissionCode, grantedBy, reason);

            _customPermissions.Add(custom);
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        #region Status management
        public void Deactivate()
        {
            Guard.Against(Status == AdminStatus.Inactive, "Admin is already inactive");
            Status = AdminStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new AdminDeactivatedEvent(Id, UserId));
        }

        public void Suspend(string reason)
        {
            Guard.Against(Status == AdminStatus.Suspended, "Admin is already suspended");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));
            Status = AdminStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new AdminSuspendedEvent(Id, UserId, reason));
        }

        public void Activate()
        {
            Guard.Against(Status == AdminStatus.Active, "Admin is already active");
            Status = AdminStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        #region Permission check helpers
        public bool HasRoleAssignment(Guid roleId) =>
            _roleAssignments.Any(r => r.RoleId == roleId && r.IsActive && !r.IsExpired());

        public bool IsActive => Status == AdminStatus.Active;
        #endregion
    }
}
