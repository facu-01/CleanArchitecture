using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions;


public abstract class BaseSpecification<TEntity, TEntityId> : ISpecification<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    public BaseSpecification()
    {
    }

    public List<Expression<Func<TEntity, bool>>> Criterias { get; } = new();

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();

    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnable { get; private set; }


    protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
    {
        Criterias.Add(criteria);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderby)
    {
        OrderBy = orderby;
    }

    protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDesc)
    {
        OrderByDescending = orderByDesc;
    }

    protected void ApplyPagin(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnable = true;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}
