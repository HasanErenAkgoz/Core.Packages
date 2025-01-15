using Core.Packages.Domain.Security.Models;

namespace Core.Packages.Security.OAuth.Services;

public interface IOAuthManager
{
    string? GetCurrentClientId();  // Token'dan client_id'yi al
    Task<OAuthClient?> GetClientByIdAsync(string clientId);
    Task<IList<OAuthClient>> GetAllClientsAsync(CancellationToken cancellationToken = default);
    Task<OAuthClient> CreateClientAsync(OAuthClient client, CancellationToken cancellationToken = default);
    Task<OAuthClient> UpdateClientAsync(OAuthClient client, CancellationToken cancellationToken = default);
} 