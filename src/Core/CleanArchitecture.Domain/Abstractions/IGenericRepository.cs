namespace CleanArchitecture.Domain.Abstractions;
public interface IGenericRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId>
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

}
