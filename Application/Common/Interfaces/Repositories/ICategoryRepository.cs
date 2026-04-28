namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<Category?> GetByOrderCodeAsync(string categoryId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> HasChildrenAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task<bool> HasProductsAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetChildrenAsync(Guid parentId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetLeafCategoriesAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Category category, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
        void Delete(Category category);
    }
}
