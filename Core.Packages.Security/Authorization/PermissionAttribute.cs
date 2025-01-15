using Microsoft.AspNetCore.Authorization;

namespace Core.Packages.Security.Authorization;

public class PermissionAttribute : AuthorizeAttribute
{
    public PermissionAttribute(string permission) : base(policy: permission)
    {
    }
} 