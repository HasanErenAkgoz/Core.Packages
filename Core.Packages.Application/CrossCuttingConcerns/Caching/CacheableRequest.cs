using MediatR;

namespace Core.Packages.Application.Pipelines.Caching;

public abstract class CacheableRequest<TResponse> : IRequest<TResponse>, ICacheableRequest
{
    public virtual string CacheKey => $"{GetType().Name}";
    public virtual bool BypassCache => false;
    public virtual string? CacheGroupKey => $"{GetType().Name.Replace("Query", "").Replace("Command", "")}";
    public virtual TimeSpan? SlidingExpiration => null;

    protected string GenerateCacheKeyFromRequest(params object[] parameters)
    {
        var key = CacheKey;
        if (parameters.Any())
            key = $"{key}_{string.Join("_", parameters)}";
        return key;
    }
} 