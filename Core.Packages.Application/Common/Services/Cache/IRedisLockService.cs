namespace Core.Packages.Application.Common.Services.Cache
{
    public interface IRedisLockService
    {
        Task<TResponse> AcquireLockAsync<TResponse>(string resource, TimeSpan expiry, Func<Task<TResponse>> action);
    }
}
