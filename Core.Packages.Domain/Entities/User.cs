using Core.Packages.Domain.Comman;

namespace Core.Packages.Domain.Entities
{
    public partial class User : BaseEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

}
