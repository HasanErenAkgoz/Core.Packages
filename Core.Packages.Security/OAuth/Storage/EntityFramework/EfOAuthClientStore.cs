using Core.Packages.Domain.Entities.Auth;
using Core.Packages.Domain.Security.Models;
using Core.Packages.Security.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Core.Packages.Security.OAuth.Storage.EntityFramework;

public class EfOAuthClientStore : IOAuthClientStore
{
    private readonly DbContext _context;
    private readonly DbSet<OAuthClientEntity> _clients;

    public EfOAuthClientStore(DbContext context)
    {
        _context = context;
        _clients = context.Set<OAuthClientEntity>();
    }

    public async Task<OAuthClient?> GetClientAsync(string clientId)
    {
        var entity = await _clients.FirstOrDefaultAsync(x => x.ClientId == clientId && x.IsActive);
        if (entity == null) return null;

        return MapToModel(entity);
    }

    public async Task SaveClientAsync(OAuthClient client)
    {
        var entity = await _clients.FirstOrDefaultAsync(x => x.ClientId == client.ClientId);
        if (entity == null)
        {
            entity = new OAuthClientEntity
            {
                ClientId = client.ClientId,
                CreatedDate = DateTime.UtcNow
            };
            _clients.Add(entity);
        }
        else
        {
            entity.ModifiedDate = DateTime.UtcNow;
        }

        entity.ClientSecret = HashingHelper.CreatePasswordHash(client.ClientSecret);
        entity.RedirectUris = JsonSerializer.Serialize(client.RedirectUris);
        entity.AllowedGrantTypes = JsonSerializer.Serialize(client.AllowedGrantTypes);
        entity.AllowedScopes = JsonSerializer.Serialize(client.AllowedScopes);
        entity.AccessTokenLifetime = client.AccessTokenLifetime;
        entity.RefreshTokenLifetime = client.RefreshTokenLifetime;
        entity.IsActive = true;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(string clientId)
    {
        var entity = await _clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
        if (entity != null)
        {
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OAuthClient>> GetClientsAsync()
    {
        var entities = await _clients.Where(x => x.IsActive).ToListAsync();
        return entities.Select(MapToModel).ToList();
    }

    public async Task<bool> ValidateClientSecretAsync(string clientId, string clientSecret)
    {
        var entity = await _clients.FirstOrDefaultAsync(x => x.ClientId == clientId && x.IsActive);
        if (entity == null) return false;

        return HashingHelper.VerifyPasswordHash(clientSecret, entity.ClientSecret);
    }

    private static OAuthClient MapToModel(OAuthClientEntity entity)
    {
        return new OAuthClient
        {
            ClientId = entity.ClientId,
            ClientSecret = string.Empty,
            RedirectUris = JsonSerializer.Deserialize<List<string>>(entity.RedirectUris) ?? new(),
            AllowedGrantTypes = JsonSerializer.Deserialize<List<string>>(entity.AllowedGrantTypes) ?? new(),
            AllowedScopes = JsonSerializer.Deserialize<List<string>>(entity.AllowedScopes) ?? new(),
            AccessTokenLifetime = entity.AccessTokenLifetime,
            RefreshTokenLifetime = entity.RefreshTokenLifetime
        };
    }
} 