using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public string? TwoFactorSecretKey { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public string? ClientId { get; set; }

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        IsTwoFactorEnabled = false;
    }

    public User(
        string userName,
        string email,
        string firstName,
        string lastName,
        string? clientId = null) : base()
    {
        UserName = userName;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        ClientId = clientId;
        IsTwoFactorEnabled = false;
        EmailConfirmed = false;
        Id = Guid.NewGuid();
    }
}