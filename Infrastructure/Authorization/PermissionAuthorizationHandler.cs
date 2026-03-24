namespace Daco.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PermissionAuthorizationHandler> _logger;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public PermissionAuthorizationHandler(
            IAdminUserRepository adminRepository,
            IMemoryCache cache,
            ILogger<PermissionAuthorizationHandler> logger)
        {
            _adminRepository = adminRepository;
            _cache = cache;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (!context.User.IsInRole(UserRoles.Admin))
            {
                context.Fail();
                return;
            }

            var adminIdClaim = context.User.FindFirstValue("admin_id");
            if (adminIdClaim is null || !Guid.TryParse(adminIdClaim, out var adminId))
            {
                context.Fail();
                return;
            }

            var permissions = await GetPermissionsAsync(adminId);

            if (permissions.Contains(requirement.Permission))
                context.Succeed(requirement);
            else
                context.Fail();
        }

        private async Task<IReadOnlyList<string>> GetPermissionsAsync(Guid adminId)
        {
            var cacheKey = $"admin_permissions:{adminId}";

            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<string>? permissions)
                && permissions is not null)
            {
                _logger.LogDebug("Permissions loaded from cache for admin {AdminId}", adminId);
                return permissions;
            }

            _logger.LogDebug("Loading permissions from DB for admin {AdminId}", adminId);

            permissions = await _adminRepository.GetPermissionsAsync(adminId);

            _cache.Set(cacheKey, permissions, CacheDuration);

            return permissions;
        }
    }
}
