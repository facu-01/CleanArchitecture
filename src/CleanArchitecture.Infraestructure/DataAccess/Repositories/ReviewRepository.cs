using CleanArchitecture.Domain.Reviews;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal sealed class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
