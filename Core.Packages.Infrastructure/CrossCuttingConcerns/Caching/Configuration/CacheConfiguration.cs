using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.Configuration;

namespace Core.Packages.Infrastructure.Caching.Configuration;

public class CacheConfiguration : ICacheConfiguration
{
    public double DefaultExpiration { get; }
    public string InstanceName { get; }
    public int RetryCount { get; }
    public TimeSpan RetryDelay { get; }
    public long DefaultCacheDuration { get; set; }

    public CacheConfiguration(IConfiguration configuration, long defaultCacheDuration)
    {
        DefaultCacheDuration = defaultCacheDuration;
        var section = configuration.GetSection("Cache");
        DefaultExpiration = double.Parse(section["DefaultExpirationMinutes"] ?? "30");
        InstanceName = section["InstanceName"] ?? "CorePackages_";
        RetryCount = int.Parse(section["RetryCount"] ?? "3");
        RetryDelay = TimeSpan.FromMilliseconds(double.Parse(section["RetryDelayMilliseconds"] ?? "1000"));
    }
} 