using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace Core.Packages.Infrastructure.CrossCuttingConcerns.Caching.Redis;

public class RedisCacheManager : ICacheManager
{
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly IDatabase _cache;
    private readonly ICacheConfiguration _configuration;
    private readonly ILogger<RedisCacheManager> _logger;

    public RedisCacheManager(
        IConnectionMultiplexer redisConnection,
        ICacheConfiguration configuration,
        ILogger<RedisCacheManager> logger)
    {
        _redisConnection = redisConnection;
        _cache = _redisConnection.GetDatabase();
        _configuration = configuration;
        _logger = logger;
    }

    public T Get<T>(string key)
    {
        try
        {
            var value = _cache.StringGet(key);
            if (!value.HasValue)
            {
                _logger.LogWarning("Cache miss for key: {Key}", key);
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting value for key: {Key}", key);
            throw;
        }
    }

    public object Get(string key)
    {
        try
        {
            var value = _cache.StringGet(key);
            if (!value.HasValue)
            {
                _logger.LogWarning("Cache miss for key: {Key}", key);
                return null;
            }
            return JsonSerializer.Deserialize<object>(value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting value for key: {Key}", key);
            throw;
        }
    }

    public void Add(string key, object value, int duration)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var serializedValue = JsonSerializer.Serialize(value, options);
            var expiry = TimeSpan.FromMinutes(duration);

            _cache.StringSet(key, serializedValue, expiry);
            _logger.LogInformation("Added cache entry for key: {Key} with duration: {Duration} minutes", key, duration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding value for key: {Key}", key);
            throw;
        }
    }

    public bool IsAdd(string key)
    {
        return _cache.KeyExists(key);
    }

    public void Remove(string key)
    {
        try
        {
            _cache.KeyDelete(key);
            _logger.LogInformation("Removed cache entry for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing key: {Key}", key);
            throw;
        }
    }

    public void RemoveByPattern(string pattern)
    {
        try
        {
            var endpoints = _redisConnection.GetEndPoints();
            var server = _redisConnection.GetServer(endpoints.First());
            var keys = server.Keys(pattern: $"*{pattern}*").ToArray();

            foreach (var key in keys)
            {
                Remove(key);
            }

            _logger.LogInformation("Removed {Count} cache entries matching pattern: {Pattern}", keys.Length, pattern);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing keys by pattern: {Pattern}", pattern);
            throw;
        }
    }
} 