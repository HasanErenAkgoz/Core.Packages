
namespace Core.Packages.Application.CrossCuttingConcerns.Caching;

public interface ICacheConfiguration
{
    double DefaultExpiration { get; }
    string InstanceName { get; }
    int RetryCount { get; }
    TimeSpan RetryDelay { get; }
    long DefaultCacheDuration { get; set; }
}