using System;

namespace Core.Packages.Domain.Security.Models;

// OAuth 2.0 Authorization Code flow için kullanılan istek modeli
public class AuthorizationRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string CodeChallenge { get; set; } = string.Empty;
    public string CodeChallengeMethod { get; set; } = "S256"; // PKCE için
    public string ResponseType { get; set; } = "code";
} 