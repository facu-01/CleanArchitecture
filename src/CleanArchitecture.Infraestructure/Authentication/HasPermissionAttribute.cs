using CleanArchitecture.Domain.Permissions;

using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infraestructure.Authentication;
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionEnum permission) :
        base(policy: permission.ToString())
    {

    }
}
