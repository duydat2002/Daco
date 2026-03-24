namespace Daco.Application.Common.Interfaces.Services.Authorization
{
    public interface IPermissionCacheService
    {
        void InvalidateCache(Guid adminId);
    }
}
