using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Core.Packages.Infrastructure.Caching.Managers;

public class RedisCacheManager : ICacheManager
{
    private readonly IDistributedCache _cache;
    private readonly ICacheConfiguration _configuration;

    public RedisCacheManager(IDistributedCache cache, ICacheConfiguration configuration)
    {
        _cache = cache;
        _configuration = configuration;
    }

    public T Get<T>(string key)
    {
        var value = _cache.GetString(key);
        return value == null ? default! : JsonSerializer.Deserialize<T>(value)!;
    }

    public object Get(string key)
    {
        return Get<object>(key)!;
    }

    public void Add(string key, object value, int duration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(duration)
        };

        var jsonData = JsonSerializer.Serialize(value);
        _cache.SetString(key, jsonData, options);
    }

    public void Add(string key, object value, DateTime expireTime)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expireTime
        };

        var jsonData = JsonSerializer.Serialize(value);
        _cache.SetString(key, jsonData, options);
    }

    public bool IsAdd(string key)
    {
        return _cache.Get(key) != null;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void RemoveByPattern(string pattern)
    {
        // Redis'te pattern ile silme işlemi için SCAN ve DEL komutları kullanılmalı
        // Bu özellik için IServer interface'i üzerinden işlem yapılması gerekiyor
        throw new NotImplementedException("Redis does not support pattern-based key removal through IDistributedCache");
    }

    public void Clear()
    {
        // Redis'te tüm verileri silmek için FLUSHDB komutu kullanılmalı
        // Bu özellik için IServer interface'i üzerinden işlem yapılması gerekiyor
        throw new NotImplementedException("Redis does not support clearing all cache through IDistributedCache");
    }
} 