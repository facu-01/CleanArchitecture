namespace CleanArchitecture.Domain.Abstractions;
public interface IGenericRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllWithSpec(ISpecification<TEntity, TEntityId> spec, CancellationToken cancellationToken);

    Task<int> CountAsync(ISpecification<TEntity, TEntityId> spec, CancellationToken cancellationToken);
}
