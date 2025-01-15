using Core.Packages.Application.Repositories;
using Core.Packages.Domain.Security.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Packages.Security.OAuth.Services;

public class OAuthManager : IOAuthManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOAuthClientRepository _clientRepository;

    public OAuthManager(
        IHttpContextAccessor httpContextAccessor,
        IOAuthClientRepository clientRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientRepository = clientRepository;
    }

    public string? GetCurrentClientId()
    {
        // Token'dan client_id claim'ini al
        return _httpContextAccessor.HttpContext?.User?.FindFirst("client_id")?.Value;
    }

    public async Task<OAuthClient?> GetClientByIdAsync(string clientId)
    {
        return await _clientRepository.GetByClientIdAsync(clientId);
    }

    public async Task<IList<OAuthClient>> GetAllClientsAsync(CancellationToken cancellationToken = default)
    {
        return await _clientRepository.GetAllAsync(cancellationToken);
    }

    public async Task<OAuthClient> CreateClientAsync(OAuthClient client, CancellationToken cancellationToken = default)
    {
        // Client secret otomatik oluştur
        if (string.IsNullOrEmpty(client.ClientSecret))
            client.ClientSecret = Guid.NewGuid().ToString("N");

        return await _clientRepository.AddAsync(client);
    }

    public async Task<OAuthClient> UpdateClientAsync(OAuthClient client, CancellationToken cancellationToken = default)
    {
        return await _clientRepository.UpdateAsync(client);
    }
} 