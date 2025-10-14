using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;
public interface IReviewRepository : IGenericRepository<Review,ReviewId>
{
}
