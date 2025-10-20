using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Roles;
public sealed class Role : Enumeration<Role>
{
    public Role(int id, string name) : base(id, name) { }

    public static readonly Role Administrator = new(1, "Administrator");
    public static readonly Role User = new(2, "User");


    public ICollection<Permission>? Permissions { get; set; }
}
