namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IAdminUserRepository
    {
        Task AddAsync(AdminUser adminUser, CancellationToken cancellationToken = default);
        Task UpdateAsync(AdminUser adminUser, CancellationToken cancellationToken = default);
        Task<AdminUser?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<AdminUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> IsAdminAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<string>> GetRolesAsync(Guid adminId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<string>> GetPermissionsAsync(Guid adminId, CancellationToken cancellationToken = default);
        Task<AdminUser?> GetByEmployeeCodeAsync(string employeeCode, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AdminRole>> GetRolesByIdsAsync(List<Guid> roleIds, CancellationToken cancellationToken = default);
        Task<AdminRole?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}
