using Core.Packages.Domain.Comman;

namespace Core.Packages.Domain.Entities
{
    public class Permission : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

}
