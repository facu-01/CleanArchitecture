using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;
public interface IUserRepository : IGenericRepository<User,UserId>
{
}
