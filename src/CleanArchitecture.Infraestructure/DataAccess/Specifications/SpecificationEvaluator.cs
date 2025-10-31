using System.Linq.Expressions;

using CleanArchitecture.Domain.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess.Specifications;

public class SpecificationEvaluator<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParam;
        private readonly ParameterExpression _newParam;

        public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
        {
            _oldParam = oldParam;
            _newParam = newParam;
        }

        protected override Expression VisitParameter(ParameterExpression node)
            => node == _oldParam ? _newParam : base.VisitParameter(node);
    }


    public static IQueryable<TEntity> GetQuery(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity, TEntityId> spec,
        bool paginated
    )
    {

        if (spec.Criterias.Any())
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var predicate = spec.Criterias
                .Select(expr =>
                    new ParameterReplacer(expr.Parameters[0], parameter)
                        .Visit(expr.Body))
                .Aggregate<Expression>((acc, body) => Expression.OrElse(acc, body));

            var finalPredicate = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);

            inputQuery = inputQuery.Where(finalPredicate);
        }

        if (spec.OrderBy is not null)
        {
            inputQuery = inputQuery.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending is not null)
        {
            inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.IsPagingEnable && paginated)
        {
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
        }

        inputQuery = spec.Includes.Aggregate(inputQuery,
        (current, include) => current.Include(include))
        .AsSplitQuery()
        .AsNoTracking();

        return inputQuery;
    }

}
