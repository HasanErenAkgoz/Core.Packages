using System.Text.Json.Serialization;

namespace Core.Packages.Domain.Security.Models;

public class OAuthUserInfo
{
    [JsonPropertyName("sub")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("given_name")]
    public string GivenName { get; set; } = null!;

    [JsonPropertyName("family_name")]
    public string FamilyName { get; set; } = null!;

    [JsonPropertyName("picture")]
    public string? Picture { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
} 