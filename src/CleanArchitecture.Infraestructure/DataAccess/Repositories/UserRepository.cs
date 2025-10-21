
using CleanArchitecture.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal sealed class UserRepository : GenericRepository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailWithRolesAsync(Domain.Users.Email email, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .Include(u => u.Roles!)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UserExistsByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().AnyAsync(u => u.Email == email, cancellationToken);
    }

}
