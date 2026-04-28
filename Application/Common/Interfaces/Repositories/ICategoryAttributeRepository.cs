namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ICategoryAttributeRepository
    {
        Task AddAsync(CategoryAttribute attribute, CancellationToken cancellationToken = default);
        Task<CategoryAttribute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CategoryAttribute?> GetByIdWithValuesAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CategoryAttribute>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task<bool> SlugExistsInCategoryAsync(Guid categoryId, string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
        void Delete(CategoryAttribute attribute);
    }
}
