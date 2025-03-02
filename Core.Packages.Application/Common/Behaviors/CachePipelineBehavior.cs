using Core.Packages.Application.Common.Attributies;
using Core.Packages.Application.Common.Services.Cache;
using Core.Packages.Application.Features.Permission.Queries.GetAll;
using Core.Packages.Application.Shared.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.Claims;

namespace Core.Packages.Application.Common.Behaviors
{
    public class CachePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRedisCacheService _cacheService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CachePipelineBehavior(IRedisCacheService cacheService, IHttpContextAccessor httpContextAccessor)
        {
            _cacheService = cacheService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cacheAttribute = request.GetType().GetCustomAttribute<CacheAttribute>();
            if (cacheAttribute == null) return await next();

            string cacheKey = GenerateCacheKey(request, cacheAttribute.CacheKey);
            var cachedResponse = await _cacheService.GetJsonAsync<IDataResult<IEnumerable<GetPermissionResponse>>>(cacheKey);
            if (cachedResponse != null)
            {
                Console.WriteLine($"[CACHE] Cache Hit: {cacheKey}");
                return (TResponse)(object)cachedResponse;
            }

            var response = await next();

            if (response is IDataResult<IEnumerable<GetPermissionResponse>> dataResult && IsValidForCaching(dataResult))
            {
                await _cacheService.SetJsonAsync(cacheKey, dataResult, TimeSpan.FromMinutes(cacheAttribute.ExpiryMinutes));
                Console.WriteLine($"[CACHE] Cache Set: {cacheKey}");
            }
            else
            {
                Console.WriteLine($"[CACHE] Skipping Cache - Response is Empty for {cacheKey}");
            }

            return response;
        }
        private string GenerateCacheKey(TRequest request, string cacheKeyTemplate)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"[CACHE] Original Template: {cacheKeyTemplate}");

            foreach (var prop in request.GetType().GetProperties())
            {
                var value = prop.GetValue(request)?.ToString() ?? string.Empty;
                cacheKeyTemplate = cacheKeyTemplate.Replace($"{{{prop.Name}}}", value);
            }

            if (cacheKeyTemplate.Contains("{UserId}") && !string.IsNullOrEmpty(userId))
            {
                cacheKeyTemplate = cacheKeyTemplate.Replace("{UserId}", userId);
            }

            Console.WriteLine($"[CACHE] Final Cache Key: {cacheKeyTemplate}");
            return cacheKeyTemplate;
        }



        private static bool IsValidForCaching<T>(IDataResult<T> result)
        {
            return result.Data != null && (!(result.Data is IEnumerable<object> list) || list.Any());
        }
    }
}