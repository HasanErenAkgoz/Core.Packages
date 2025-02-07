using Core.Packages.Domain.Comman;

namespace Core.Packages.Domain.Entities
{
    public class UserRole : BaseEntity<int>
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

}
