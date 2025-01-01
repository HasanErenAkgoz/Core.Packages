using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Core.Packages.Infrastructure.Caching;

public class MemoryCacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly ICacheConfiguration _configuration;
    private readonly ILogger<MemoryCacheManager> _logger;

    public MemoryCacheManager(
        IMemoryCache memoryCache,
        ICacheConfiguration configuration,
        ILogger<MemoryCacheManager> logger)
    {
        _memoryCache = memoryCache;
        _configuration = configuration;
        _logger = logger;
    }

    public T Get<T>(string key)
    {
        try
        {
            var value = _memoryCache.Get<T>(key);
            if (value == null)
            {
                _logger.LogWarning("Cache miss for key: {Key}", key);
            }
            return value;
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
            var value = _memoryCache.Get(key);
            if (value == null)
            {
                _logger.LogWarning("Cache miss for key: {Key}", key);
            }
            return value;
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
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(duration)
            };
            _memoryCache.Set(key, value, cacheEntryOptions);
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
        return _memoryCache.TryGetValue(key, out _);
    }

    public void Remove(string key)
    {
        try
        {
            _memoryCache.Remove(key);
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
            if (string.IsNullOrWhiteSpace(pattern))
            {
                _logger.LogWarning("RemoveByPattern called with null or empty pattern");
                return;
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = GetAllKeys().Where(key => regex.IsMatch(key)).ToList();

            foreach (var key in keysToRemove)
            {
                Remove(key);
            }

            _logger.LogInformation("Removed {Count} cache entries matching pattern: {Pattern}", keysToRemove.Count, pattern);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing keys by pattern: {Pattern}", pattern);
            throw;
        }
    }

    private IEnumerable<string> GetAllKeys()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var entries = _memoryCache.GetType().GetField("_entries", flags)?.GetValue(_memoryCache);
        
        if (entries == null)
        {
            _logger.LogWarning("Could not access cache entries through reflection");
            return Enumerable.Empty<string>();
        }

        var cacheItems = entries.GetType()
            .GetProperty("Keys", BindingFlags.Instance | BindingFlags.Public)?
            .GetValue(entries) as ICollection;

        return cacheItems?.Cast<string>() ?? Enumerable.Empty<string>();
    }
}