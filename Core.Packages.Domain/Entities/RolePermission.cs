using Core.Packages.Domain.Comman;

namespace Core.Packages.Domain.Entities
{
    public class RolePermission : BaseEntity<int>
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }

}
