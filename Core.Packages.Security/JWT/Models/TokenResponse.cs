namespace Core.Packages.Security.JWT.Models;

// JWT token yanıtı için model
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
} 