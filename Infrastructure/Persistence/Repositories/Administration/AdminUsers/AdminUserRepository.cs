namespace Daco.Infrastructure.Persistence.Repositories.Administration.AdminUsers
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<AdminUserRepository> _logger;

        public AdminUserRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<AdminUserRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(AdminUser adminUser, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, adminUser,
                async () => await _context.AdminUsers.AddAsync(adminUser, cancellationToken));
        }

        public async Task UpdateAsync(AdminUser adminUser, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, adminUser,
                () =>
                {
                    _context.AdminUsers.Update(adminUser);
                    return Task.CompletedTask;
                });
        }

        public async Task<AdminUser?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { userId },
                () => _context.AdminUsers
                    .FirstOrDefaultAsync(s => s.UserId == userId,
                        cancellationToken));
        }

        public async Task<AdminUser?> GetByIdAsync(Guid adminId, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { adminId },
                () => _context.AdminUsers
                    .FirstOrDefaultAsync(s => s.Id == adminId,
                        cancellationToken));
        }

        public async Task<bool> IsAdminAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { userId },
                () => _context.AdminUsers
                    .AnyAsync(s => s.UserId == userId
                        && s.Status == AdminStatus.Active,
                    cancellationToken));
        }

        public async Task<IReadOnlyList<string>> GetRolesAsync(
            Guid adminId,
            CancellationToken cancellationToken = default)
        {
            return await _context.AdminRoleAssignments
                .Where(r => r.AdminId == adminId
                         && r.IsActive
                         && (r.ExpiresAt == null || r.ExpiresAt > DateTime.UtcNow))
                .Join(_context.AdminRoles,
                    r => r.RoleId,
                    role => role.Id,
                    (r, role) => role.RoleCode)
                .ToListAsync(cancellationToken) ?? new List<string>();
        }

        public async Task<IReadOnlyList<string>> GetPermissionsAsync(
            Guid adminId,
            CancellationToken cancellationToken = default)
        {
            // Permissions từ roles
            var rolePermissions = await _context.Set<AdminRoleAssignment>()
                .Where(r => r.AdminId == adminId
                         && r.IsActive
                         && (r.ExpiresAt == null || r.ExpiresAt > DateTime.UtcNow))
                .Join(_context.RolePermissions,
                    r => r.RoleId,
                    rp => rp.RoleId,
                    (r, rp) => rp.PermissionId)
                .Join(_context.AdminPermissions,
                    permId => permId,
                    p => p.Id,
                    (permId, p) => p.PermissionCode)
                .Distinct()
                .ToListAsync(cancellationToken);

            // Custom permissions (grant hoặc revoke)
            var customPermissions = await _context.Set<AdminCustomPermission>()
                .Where(c => c.AdminId == adminId)
                .Join(_context.AdminPermissions,
                    c => c.PermissionId,
                    p => p.Id,
                    (c, p) => new { p.PermissionCode, c.IsGranted })
                .ToListAsync(cancellationToken);

            // Apply custom: grant thêm, revoke bớt
            var granted = customPermissions.Where(c => c.IsGranted).Select(c => c.PermissionCode);
            var revoked = customPermissions.Where(c => !c.IsGranted).Select(c => c.PermissionCode).ToHashSet();

            return rolePermissions
                .Union(granted)
                .Where(p => !revoked.Contains(p))
                .ToList()
                .AsReadOnly();
        }

        public async Task<AdminUser?> GetByEmployeeCodeAsync(
            string employeeCode,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { employeeCode },
                () => _context.AdminUsers
                    .FirstOrDefaultAsync(a => a.EmployeeCode == employeeCode, cancellationToken));
        }

        public async Task<AdminRole?> GetRoleByIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { roleId },
                () => _context.AdminRoles
                    .FirstOrDefaultAsync(r => r.Id == roleId && r.IsActive, cancellationToken));
        }

        public async Task<IReadOnlyList<AdminRole>> GetRolesByIdsAsync(
            List<Guid> roleIds,
            CancellationToken cancellationToken = default)
        {
            return await _context.AdminRoles
                .Where(r => roleIds.Contains(r.Id) && r.IsActive)
                .ToListAsync(cancellationToken);
        }

        #region Admin Role
        public async Task<AdminRoleAssignment?> GetRoleAssignmentAsync(
            Guid adminId,
            Guid roleId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<AdminRoleAssignment>()
                .FirstOrDefaultAsync(r => r.AdminId == adminId
                                       && r.RoleId == roleId, 
                                       cancellationToken);
        }

        public async Task AddRoleAssignmentAsync(AdminRoleAssignment assignment, CancellationToken cancellationToken = default)
        {
            await _context.AdminRoleAssignments.AddAsync(assignment, cancellationToken);
        }

        public async Task RemoveRoleAssignmentAsync(AdminRoleAssignment assignment, CancellationToken cancellationToken = default)
        {
            _context.AdminRoleAssignments.Remove(assignment);
            await Task.CompletedTask;
        }

        public async Task AddCustomPermissionAsync(AdminCustomPermission permission, CancellationToken cancellationToken = default)
        {
            await _context.AdminCustomPermissions.AddAsync(permission, cancellationToken);
        }
        #endregion
        #endregion
    }
}
