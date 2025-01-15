using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace Core.Packages.Infrastructure.Caching.Managers;

public class MemoryCacheManager : ICacheManager
{
    private readonly IMemoryCache _cache;
    private readonly ICacheConfiguration _configuration;

    public MemoryCacheManager(IMemoryCache cache, ICacheConfiguration configuration)
    {
        _cache = cache;
        _configuration = configuration;
    }

    public T Get<T>(string key)
    {
        return _cache.Get<T>(key)!;
    }

    public object Get(string key)
    {
        return _cache.Get(key)!;
    }

    public void Add(string key, object value, int duration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(duration),
            SlidingExpiration = TimeSpan.FromMinutes(_configuration.SlidingExpirationInMinutes),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(key, value, options);
    }

    public void Add(string key, object value, DateTime expireTime)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = expireTime,
            SlidingExpiration = TimeSpan.FromMinutes(_configuration.SlidingExpirationInMinutes),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(key, value, options);
    }

    public bool IsAdd(string key)
    {
        return _cache.TryGetValue(key, out _);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void RemoveByPattern(string pattern)
    {
        var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var keysToRemove = new List<string>();

        // MemoryCache'deki tüm key'leri almak için reflection kullanıyoruz
        var cacheEntriesField = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var cacheEntries = cacheEntriesField?.GetValue(_cache) as dynamic;

        if (cacheEntries != null)
        {
            foreach (var cacheEntry in cacheEntries)
            {
                var key = cacheEntry.GetType().GetProperty("Key")?.GetValue(cacheEntry) as string;
                if (key != null && regex.IsMatch(key))
                {
                    keysToRemove.Add(key);
                }
            }
        }

        foreach (var key in keysToRemove)
        {
            Remove(key);
        }
    }

    public void Clear()
    {
        // MemoryCache'i temizlemek için yeni bir instance oluşturuyoruz
        if (_cache is MemoryCache memoryCache)
        {
            memoryCache.Dispose();
        }
    }
} 