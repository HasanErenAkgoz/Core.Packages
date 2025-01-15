using System.Text.Json.Serialization;

namespace Core.Packages.Domain.Security.Models;

// OAuth token bilgilerini tutan sınıf
public class OAuthToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
    
    [JsonPropertyName("id_token")]
    public string? IdToken { get; set; }
} 