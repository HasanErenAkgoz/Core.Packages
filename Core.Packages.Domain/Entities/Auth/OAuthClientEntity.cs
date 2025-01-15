using Core.Packages.Domain.Common;

namespace Core.Packages.Domain.Entities.Auth;

public class OAuthClientEntity : Entity<int>
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string AllowedScopes { get; set; } = "[]"; // JSON array olarak saklanacak
    public string RedirectUris { get; set; } = "[]"; // JSON array olarak saklanacak
    public string AllowedGrantTypes { get; set; } = "[]"; // JSON array olarak saklanacak
    public bool RequirePkce { get; set; }
    public int AccessTokenLifetime { get; set; } = 3600; // 1 saat
    public int RefreshTokenLifetime { get; set; } = 2592000; // 30 gün
    public bool IsActive { get; set; } = true;
}