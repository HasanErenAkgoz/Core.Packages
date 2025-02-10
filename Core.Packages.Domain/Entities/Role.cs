using Core.Packages.Domain.Comman;

namespace Core.Packages.Domain.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

}
