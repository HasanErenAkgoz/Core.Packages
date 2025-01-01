using System;

namespace Core.Packages.Application.Security.Tokens;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
}