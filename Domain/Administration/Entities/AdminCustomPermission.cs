namespace Daco.Domain.Administration.Entities
{
    public class AdminCustomPermission : Entity
    {
        public Guid     AdminId        { get; private set; }
        public Guid     PermissionId   { get; private set; }
        public string   PermissionCode { get; private set; }  // snapshot
        public bool     IsGranted      { get; private set; }  // true=grant, false=revoke
        public Guid     GrantedBy      { get; private set; }
        public string?  Reason         { get; private set; }
        public DateTime GrantedAt      { get; private set; }

        protected AdminCustomPermission() { }

        public static AdminCustomPermission Grant(
            Guid adminId,
            Guid permissionId,
            string permissionCode,
            Guid grantedBy,
            string? reason = null)
        {
            return new AdminCustomPermission
            {
                Id = Guid.NewGuid(),
                AdminId = adminId,
                PermissionId = permissionId,
                PermissionCode = permissionCode,
                IsGranted = true,
                GrantedBy = grantedBy,
                Reason = reason,
                GrantedAt = DateTime.UtcNow
            };
        }

        public static AdminCustomPermission Revoke(
            Guid adminId,
            Guid permissionId,
            string permissionCode,
            Guid grantedBy,
            string? reason = null)
        {
            return new AdminCustomPermission
            {
                Id = Guid.NewGuid(),
                AdminId = adminId,
                PermissionId = permissionId,
                PermissionCode = permissionCode,
                IsGranted = false,
                GrantedBy = grantedBy,
                Reason = reason,
                GrantedAt = DateTime.UtcNow
            };
        }
    }
}
