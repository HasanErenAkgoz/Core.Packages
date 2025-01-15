using Core.Packages.Application.Pipelines.Caching;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Packages.Infrastructure.Caching;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheableRequest
{
    private readonly DistributedCacheService _cacheService;
    private readonly CacheSettings _settings;

    public CachingBehavior(DistributedCacheService cacheService, IConfiguration configuration)
    {
        _cacheService = cacheService;
        _settings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();

        var cachedResponse = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
        if (cachedResponse != null)
            return cachedResponse;

        var response = await next();

        var slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromMinutes(_settings.SlidingExpiration);
        await _cacheService.SetAsync(request.CacheKey, response, slidingExpiration, cancellationToken);

        return response;
    }
} 