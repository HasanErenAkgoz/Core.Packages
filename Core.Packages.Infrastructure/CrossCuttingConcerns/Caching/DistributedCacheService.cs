using Core.Packages.Application.Pipelines.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Core.Packages.Infrastructure.Caching;

public class DistributedCacheService : ICacheRemover
{
    private readonly IDistributedCache _cache;

    public DistributedCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetAsync(key, cancellationToken);
        if (data == null)
            return default;

        var jsonString = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = slidingExpiration
        };

        var jsonString = JsonSerializer.Serialize(value);
        var data = Encoding.UTF8.GetBytes(jsonString);
        await _cache.SetAsync(key, data, options, cancellationToken);

        if (!string.IsNullOrEmpty(GetGroupKey(key)))
            await AddKeyToGroupAsync(GetGroupKey(key)!, key, slidingExpiration, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }

    public async Task RemoveByGroupAsync(string groupKey, CancellationToken cancellationToken = default)
    {
        var group = await GetGroupAsync(groupKey, cancellationToken);
        if (group != null)
        {
            foreach (var key in group)
                await RemoveAsync(key, cancellationToken);
            
            await RemoveAsync($"Group_{groupKey}", cancellationToken);
        }
    }

    private async Task AddKeyToGroupAsync(string groupKey, string key, TimeSpan? slidingExpiration, CancellationToken cancellationToken)
    {
        var group = await GetGroupAsync(groupKey, cancellationToken) ?? new List<string>();
        
        if (!group.Contains(key))
        {
            group = group.Append(key).ToList();
            await SetAsync($"Group_{groupKey}", group, slidingExpiration, cancellationToken);
        }
    }

    private async Task<List<string>?> GetGroupAsync(string groupKey, CancellationToken cancellationToken)
    {
        return await GetAsync<List<string>>($"Group_{groupKey}", cancellationToken);
    }

    private string? GetGroupKey(string key)
    {
        var parts = key.Split('_');
        return parts.Length > 1 ? parts[0] : null;
    }
} 