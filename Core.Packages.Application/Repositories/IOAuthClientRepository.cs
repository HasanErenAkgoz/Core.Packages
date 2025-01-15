using Core.Packages.Domain.Security.Models;

namespace Core.Packages.Application.Repositories;

public interface IOAuthClientRepository
{
    Task<OAuthClient?> GetByClientIdAsync(string clientId);
    Task<IList<OAuthClient>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OAuthClient> AddAsync(OAuthClient client);
    Task<OAuthClient> UpdateAsync(OAuthClient client);
    Task<bool> DeleteAsync(string clientId);
} 