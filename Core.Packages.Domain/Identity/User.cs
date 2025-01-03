using Core.Packages.Domain.Common;

namespace Core.Packages.Domain.Identity;

public class User : BaseEntity<int>, IEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpires { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public DateTime? EmailConfirmationTokenExpires { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

    public User()
    {
        UserRoles = new HashSet<UserRole>();
        RefreshTokens = new HashSet<RefreshToken>();
        IsActive = true;
        EmailConfirmed = false;
    }
} 