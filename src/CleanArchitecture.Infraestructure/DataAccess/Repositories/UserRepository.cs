using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal sealed class UserRepository : GenericRepository<User,UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
