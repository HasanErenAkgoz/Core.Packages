
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Domain.Entities.Identity;

public class Role : IdentityRole<Guid>
{
    public ICollection<RolePermission> RolePermissions { get; set; }

    public Role()
    {
        Id = Guid.NewGuid();
        RolePermissions = new HashSet<RolePermission>();
    }

    public Role(string roleName) : this()
    {
        Name = roleName;
        NormalizedName = roleName.ToUpper();
    }
} 