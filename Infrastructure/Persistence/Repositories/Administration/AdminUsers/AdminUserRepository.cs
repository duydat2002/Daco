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

            var customPermissions = await _context.Set<AdminCustomPermission>()
                .Where(c => c.AdminId == adminId)
                .Join(_context.AdminPermissions,
                    c => c.PermissionId,
                    p => p.Id,
                    (c, p) => new { p.PermissionCode, c.IsGranted })
                .ToListAsync(cancellationToken);

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

        public async Task<PagedResult<AdminListItemDTO>> GetAdminsAsync(
            GetAdminsQuery query,
            CancellationToken cancellationToken = default)
        {
            var q = _context.AdminUsers
                .Join(_context.Users,
                    a => a.UserId,
                    u => u.Id,
                    (a, u) => new { Admin = a, User = u })
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.ToLower();
                q = q.Where(x =>
                    x.User.Username.Value.Contains(search) ||
                    x.User.Email.Value.Contains(search) ||
                    x.Admin.EmployeeCode!.Contains(search));
            }

            if (!string.IsNullOrEmpty(query.Status) &&
                Enum.TryParse<AdminStatus>(query.Status, true, out var status))
            {
                q = q.Where(x => x.Admin.Status == status);
            }

            if (!string.IsNullOrEmpty(query.Department))
            {
                q = q.Where(x => x.Admin.Department == query.Department);
            }

            var total = await q.CountAsync(cancellationToken);

            var admins = await q
                .OrderByDescending(x => x.Admin.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new
                {
                    x.Admin.Id,
                    x.Admin.UserId,
                    x.Admin.EmployeeCode,
                    x.Admin.Department,
                    x.Admin.Position,
                    x.Admin.Status,
                    x.Admin.CreatedAt,
                    x.Admin.UpdatedAt,
                    Username = x.User.Username.Value,
                    Email = x.User.Email.Value,
                })
                .ToListAsync(cancellationToken);

            var adminIds = admins.Select(a => a.Id).ToList();
            var roleAssignments = await _context.AdminRoleAssignments
                .Where(r => adminIds.Contains(r.AdminId)
                         && r.IsActive
                         && (r.ExpiresAt == null || r.ExpiresAt > DateTime.UtcNow))
                .Join(_context.AdminRoles,
                    r => r.RoleId,
                    role => role.Id,
                    (r, role) => new { r.AdminId, role.RoleCode })
                .ToListAsync(cancellationToken);

            var roleMap = roleAssignments
                .GroupBy(r => r.AdminId)
                .ToDictionary(g => g.Key, g => g.Select(r => r.RoleCode).ToList());

            var items = admins.Select(a => new AdminListItemDTO
            {
                Id = a.Id,
                UserId = a.UserId,
                Username = a.Username,
                Email = a.Email,
                EmployeeCode = a.EmployeeCode,
                Department = a.Department,
                Position = a.Position,
                Status = a.Status.ToString().ToLower(),
                CreatedAt = a.CreatedAt,
                Roles = roleMap.TryGetValue(a.Id, out var roles) ? roles : new()
            }).ToList();

            return new PagedResult<AdminListItemDTO>
            {
                Items = items,
                Total = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<AdminDetailDTO?> GetAdminDetailAsync(
            Guid adminId,
            CancellationToken cancellationToken = default)
        {
            var admin = await _context.AdminUsers
                .Join(_context.Users,
                    a => a.UserId,
                    u => u.Id,
                    (a, u) => new { Admin = a, User = u })
                .Where(x => x.Admin.Id == adminId)
                .Select(x => new
                {
                    x.Admin.Id,
                    x.Admin.UserId,
                    x.Admin.EmployeeCode,
                    x.Admin.Department,
                    x.Admin.Position,
                    x.Admin.WorkEmail,
                    x.Admin.WorkPhone,
                    x.Admin.Status,
                    x.Admin.Notes,
                    x.Admin.CreatedAt,
                    x.Admin.UpdatedAt,
                    Username = x.User.Username.Value,
                    Email = x.User.Email.Value,
                    Phone = x.User.Phone.Value,
                    Avatar = x.User.Avatar,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (admin is null) return null;

            var roles = await _context.AdminRoleAssignments
                .Where(r => r.AdminId == adminId
                         && r.IsActive
                         && (r.ExpiresAt == null || r.ExpiresAt > DateTime.UtcNow))
                .Join(_context.AdminRoles,
                    r => r.RoleId,
                    role => role.Id,
                    (r, role) => new RoleDTO
                    {
                        Id = role.Id,
                        RoleCode = role.RoleCode,
                        RoleName = role.RoleName,
                        AssignedAt = r.AssignedAt,
                        ExpiresAt = r.ExpiresAt
                    })
                .ToListAsync(cancellationToken);

            var roleIds = roles.Select(r => r.Id).ToList();
            var rolePermissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Join(_context.AdminPermissions,
                    rp => rp.PermissionId,
                    p => p.Id,
                    (rp, p) => new PermissionDTO
                    {
                        PermissionCode = p.PermissionCode,
                        Module = p.Module,
                        IsCustom = false,
                        IsGranted = true
                    })
                .Distinct()
                .ToListAsync(cancellationToken);

            var customPermissions = await _context.AdminCustomPermissions
                .Where(c => c.AdminId == adminId)
                .Join(_context.AdminPermissions,
                    c => c.PermissionId,
                    p => p.Id,
                    (c, p) => new PermissionDTO
                    {
                        PermissionCode = p.PermissionCode,
                        Module = p.Module,
                        IsCustom = true,
                        IsGranted = c.IsGranted
                    })
                .ToListAsync(cancellationToken);

            var revokedCodes = customPermissions
                .Where(c => !c.IsGranted)
                .Select(c => c.PermissionCode)
                .ToHashSet();

            var grantedCustom = customPermissions.Where(c => c.IsGranted).ToList();

            var finalPermissions = rolePermissions
                .Where(p => !revokedCodes.Contains(p.PermissionCode))
                .Union(grantedCustom)
                .ToList();

            return new AdminDetailDTO
            {
                Id = admin.Id,
                UserId = admin.UserId,
                Username = admin.Username,
                Email = admin.Email,
                Phone = admin.Phone,
                Avatar = admin.Avatar,
                EmployeeCode = admin.EmployeeCode,
                Department = admin.Department,
                Position = admin.Position,
                WorkEmail = admin.WorkEmail,
                WorkPhone = admin.WorkPhone,
                Status = admin.Status.ToString().ToLower(),
                Notes = admin.Notes,
                CreatedAt = admin.CreatedAt,
                UpdatedAt = admin.UpdatedAt,
                Roles = roles,
                Permissions = finalPermissions
            };
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

        #region User Management
        public async Task<PagedResult<UserListItemDTO>> GetUsersAsync(GetUsersQuery query, CancellationToken cancellationToken = default)
        {
            var q = _context.Users
                .Where(u => u.DeletedAt == null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.Trim().ToLowerInvariant();
                q = q.Where(u =>
                    u.Username.Value.Contains(search) ||
                    u.Email.Value.Contains(search) ||
                    u.Phone.Value.Contains(search) ||
                    u.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(query.Status) &&
                Enum.TryParse<UserStatus>(query.Status, true, out var status))
            {
                q = q.Where(u => u.Status == status);
            }

            var total = await q.CountAsync(cancellationToken);

            q = query.SortBy?.ToLower() switch
            {
                "username" => query.SortOrder == "asc"
                    ? q.OrderBy(u => u.Username.Value)
                    : q.OrderByDescending(u => u.Username.Value),
                _ => query.SortOrder == "asc"
                    ? q.OrderBy(u => u.CreatedAt)
                    : q.OrderByDescending(u => u.CreatedAt)
            };

            var users = await q
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new
                {
                    u.Id,
                    Username = u.Username.Value,
                    Email = u.Email.Value,
                    Phone = u.Phone.Value,
                    u.Name,
                    u.Avatar,
                    u.Status,
                    u.EmailVerified,
                    u.PhoneVerified,
                    u.Gender,
                    u.CreatedAt,
                    u.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            var userIds = users.Select(u => u.Id).ToList();
            var sellerIds = await _context.Sellers
                .Where(s => userIds.Contains(s.UserId)
                            && s.Status == SellerStatus.Active
                            && s.DeletedAt == null)
                .Select(s => s.UserId)
                .ToListAsync(cancellationToken);

            var items = users.Select(u => new UserListItemDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Phone = u.Phone,
                Name = u.Name,
                Avatar = u.Avatar,
                Status = u.Status.ToString().ToLower(),
                EmailVerified = u.EmailVerified,
                PhoneVerified = u.PhoneVerified,
                Gender = u.Gender.ToString().ToLower(),
                IsSeller = sellerIds.Contains(u.Id),
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();

            return new PagedResult<UserListItemDTO>
            {
                Items = items,
                Total = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<UserDetailDTO?> GetUserDetailAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .Include(u => u.AuthProviders)
                .Where(u => u.Id == userId && u.DeletedAt == null)
                .Select(u => new
                {
                    u.Id,
                    Username = u.Username.Value,
                    Email = u.Email.Value,
                    Phone = u.Phone.Value,
                    u.Name,
                    u.Avatar,
                    u.Gender,
                    u.DateOfBirth,
                    u.Status,
                    u.EmailVerified,
                    u.PhoneVerified,
                    u.CreatedAt,
                    u.UpdatedAt,
                    AuthProviders = u.AuthProviders
                        .Where(p => p.DeletedAt == null)
                        .Select(p => new UserAuthProviderDTO
                        {
                            ProviderType = p.ProviderType,
                            IsVerified = p.IsVerified,
                            CreatedAt = p.CreatedAt
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null) return null;

            var isSeller = await _context.Sellers
                .AnyAsync(s => s.UserId == userId
                            && s.Status == SellerStatus.Active
                            && s.DeletedAt == null, cancellationToken);

            var isAdmin = await _context.AdminUsers
                .AnyAsync(a => a.UserId == userId
                            && a.Status == AdminStatus.Active, cancellationToken);

            var totalOrders = await _context.Orders
                .CountAsync(o => o.UserId == userId, cancellationToken);

            var totalAddresses = await _context.UserAddresses
                .CountAsync(a => a.UserId == userId && a.DeletedAt == null, cancellationToken);

            var totalBankAccounts = await _context.BankAccounts
                .CountAsync(b => b.UserId == userId && b.DeletedAt == null, cancellationToken);

            return new UserDetailDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Name = user.Name,
                Avatar = user.Avatar,
                Gender = user.Gender.ToString().ToLower(),
                DateOfBirth = user.DateOfBirth?.ToString("dd/MM/yyyy"),
                Status = user.Status.ToString().ToLower(),
                EmailVerified = user.EmailVerified,
                PhoneVerified = user.PhoneVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                AuthProviders = user.AuthProviders,
                IsSeller = isSeller,
                IsAdmin = isAdmin,
                TotalOrders = totalOrders,
                TotalAddresses = totalAddresses,
                TotalBankAccounts = totalBankAccounts
            };
        }

        
        #endregion
        #endregion
    }
}
