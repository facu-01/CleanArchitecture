using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;
public interface IUserRepository : IGenericRepository<User, UserId>
{

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);

    Task<bool> UserExistsByEmailAsync(Email email, CancellationToken cancellationToken);

}
