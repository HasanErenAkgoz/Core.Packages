using Core.Packages.Domain.Common;

namespace Core.Packages.Domain.Identity;

public class UserRole : BaseEntity<int>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
} 