namespace Core.Packages.Domain.Security.Models;

public class AuthorizationCode
{
    public string Code { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string Scope { get; set; } = null!;
    public string State { get; set; } = null!;
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}