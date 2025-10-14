
using CleanArchitecture.Domain.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal abstract class GenericRepository<TEntity,TEntityId> : IGenericRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    protected readonly ApplicationDbContext _dbContext;

    protected GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
