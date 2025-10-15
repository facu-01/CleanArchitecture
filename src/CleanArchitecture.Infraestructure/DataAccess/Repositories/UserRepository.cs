
using CleanArchitecture.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal sealed class UserRepository : GenericRepository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
