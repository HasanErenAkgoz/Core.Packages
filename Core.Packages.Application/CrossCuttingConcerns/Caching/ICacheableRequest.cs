namespace Core.Packages.Application.Pipelines.Caching;

public interface ICacheableRequest
{
    string CacheKey { get; }
    bool BypassCache { get; }
    string? CacheGroupKey { get; }
    TimeSpan? SlidingExpiration { get; }
} 