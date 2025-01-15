using Core.Packages.Domain.Common;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Security.Models;

namespace Core.Packages.Security.OAuth.Storage;

// OAuth istemci saklama işlemleri için arayüz
public interface IOAuthClientStore
{
    // İstemci bilgilerini getir
    Task<OAuthClient> GetClientAsync(string clientId);

    // İstemci gizli anahtarını doğrula
    Task<bool> ValidateClientSecretAsync(string clientId, string clientSecret);

    // İstemci kaydet
    Task SaveClientAsync(OAuthClient client);

    // İstemci sil
    Task DeleteClientAsync(string clientId);

    // Tüm istemcileri getir
    Task<IEnumerable<OAuthClient>> GetClientsAsync();
}

