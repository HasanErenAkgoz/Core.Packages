namespace Core.Packages.Domain.Security.Models;

// OAuth istemci bilgilerini tutan sınıf
public class OAuthClient
{
    // İstemci kimliği
    public string ClientId { get; set; } = null!;
    
    // İstemci gizli anahtarı
    public string ClientSecret { get; set; } = null!;
    
    // İstemci adı
    public string Name { get; set; } = null!;
    
    // İstemci açıklaması
    public string Description { get; set; } = string.Empty;
    
    // İzin verilen kapsamlar
    public List<string> AllowedScopes { get; set; } = new();
    
    // İzin verilen yönlendirme URI'ları
    public List<string> RedirectUris { get; set; } = new();
    
    // İzin verilen OAuth akışları
    public List<string> AllowedGrantTypes { get; set; } = new();
    
    // PKCE gereksinimi
    public bool RequirePkce { get; set; }
    
    // Access token süresi (dakika)
    public int AccessTokenLifetime { get; set; } = 3600; // 1 saat
    
    // Refresh token süresi (gün)
    public int RefreshTokenLifetime { get; set; } = 2592000; // 30 gün
    public bool IsActive { get; set; }
} 