namespace Daco.Infrastructure.Services.Authorization
{
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly IMemoryCache _cache;

        public PermissionCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void InvalidateCache(Guid adminId)
        {
            _cache.Remove($"admin_permissions:{adminId}");
        }
    }
}
