using Core.Packages.Application.Services.Auth;
using Core.Packages.Domain.Security.Models;
using Core.Packages.Domain.Security.Models.Configurations;
using Core.Packages.Security.OAuth.Exceptions;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Core.Packages.Security.OAuth.Services;

public class GoogleOAuthService : IOAuthService
{
    private readonly HttpClient _httpClient;
    private readonly OAuthOptions _options;
    private readonly Dictionary<string, DateTime> _stateStore = new();

    public GoogleOAuthService(HttpClient httpClient, IOptions<OAuthOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> CreateAuthorizationCodeAsync(string clientId, string redirectUri, string scope, string state)
    {
        // State'i sakla ve 10 dakika geçerli olsun
        _stateStore[state] = DateTime.UtcNow.AddMinutes(10);

        var queryParams = new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["redirect_uri"] = redirectUri,
            ["response_type"] = "code",
            ["scope"] = scope,
            ["state"] = state,
            ["access_type"] = "offline",
            ["prompt"] = "consent"
        };

        var queryString = string.Join("&", queryParams.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
        return $"{_options.AuthorizationEndpoint}?{queryString}";
    }

    public async Task<OAuthToken> ExchangeCodeForTokenAsync(string code, string clientId, string clientSecret, string redirectUri)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["code"] = code,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = redirectUri
        });

        var response = await _httpClient.PostAsync(_options.TokenEndpoint, content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<OAuthToken>();
        return tokenResponse ?? throw new OAuthException("Token response is null");
    }

    public async Task<OAuthToken> CreatePasswordTokenAsync(string clientId, string clientSecret, string username, string password, string scope)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["grant_type"] = "password",
            ["username"] = username,
            ["password"] = password,
            ["scope"] = scope
        });

        var response = await _httpClient.PostAsync(_options.TokenEndpoint, content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<OAuthToken>();
        return tokenResponse ?? throw new OAuthException("Token response is null");
    }

    public async Task<OAuthToken> CreateClientCredentialsTokenAsync(string clientId, string clientSecret, string scope)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["grant_type"] = "client_credentials",
            ["scope"] = scope
        });

        var response = await _httpClient.PostAsync(_options.TokenEndpoint, content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<OAuthToken>();
        return tokenResponse ?? throw new OAuthException("Token response is null");
    }

    public async Task<OAuthToken> RefreshTokenAsync(string refreshToken, string clientId, string clientSecret)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["refresh_token"] = refreshToken,
            ["grant_type"] = "refresh_token"
        });

        var response = await _httpClient.PostAsync(_options.TokenEndpoint, content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<OAuthToken>();
        return tokenResponse ?? throw new OAuthException("Token response is null");
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://oauth2.googleapis.com/tokeninfo");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task RevokeTokenAsync(string token)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["token"] = token
        });

        var response = await _httpClient.PostAsync("https://oauth2.googleapis.com/revoke", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<OAuthUserInfo> GetUserInfoAsync(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var response = await _httpClient.GetAsync(_options.UserInformationEndpoint);
        response.EnsureSuccessStatusCode();

        var userInfo = await response.Content.ReadFromJsonAsync<OAuthUserInfo>();
        return userInfo ?? throw new OAuthException("User info response is null");
    }

    private bool ValidateState(string state)
    {
        if (!_stateStore.TryGetValue(state, out var expiresAt))
            return false;

        if (DateTime.UtcNow > expiresAt)
        {
            _stateStore.Remove(state);
            return false;
        }

        _stateStore.Remove(state);
        return true;
    }

    private static string GenerateRandomState()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
} 