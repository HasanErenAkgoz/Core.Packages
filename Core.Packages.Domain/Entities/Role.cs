using Core.Packages.Domain.Comman;
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

}
