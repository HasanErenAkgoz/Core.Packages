using Core.Packages.Domain.Common;
using Core.Packages.Domain.Security.Permissions.Entities;

namespace Core.Packages.Domain.Identity;

public class Role : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; }
} 