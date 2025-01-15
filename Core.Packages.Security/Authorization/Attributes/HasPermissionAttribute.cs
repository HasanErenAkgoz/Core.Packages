using Microsoft.AspNetCore.Authorization;

namespace Core.Packages.Security.Authorization.Attributes;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(policy: permission)
    {
    }
} 