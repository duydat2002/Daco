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
            return await _context.Set<AdminRoleAssignment>()
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
        #endregion
    }
}
