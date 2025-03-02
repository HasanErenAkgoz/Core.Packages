using Core.Packages.Application.Common.Attributies;
using Core.Packages.Application.Common.Services.Cache;
using MediatR;
using System.Reflection;

namespace Core.Packages.Application.Common.Behaviors
{
    public class LockPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRedisLockService _lockService;

        public LockPipelineBehavior(IRedisLockService lockService)
        {
            _lockService = lockService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var lockAttribute = request.GetType().GetCustomAttribute<LockAttribute>();
            if (lockAttribute == null) return await next();

            // Dinamik Lock Key oluşturma
            string dynamicLockKey = lockAttribute.LockKey;
            foreach (var prop in request.GetType().GetProperties())
            {
                dynamicLockKey = dynamicLockKey.Replace($"{{{prop.Name}}}", prop.GetValue(request)?.ToString());
            }

            TimeSpan lockTime = TimeSpan.FromSeconds(lockAttribute.LockSeconds);

            if (lockAttribute.WaitForUnlock)
            {
                while (!await _lockService.AcquireLockAsync<bool>(dynamicLockKey, lockTime, async () => true))
                {
                    await Task.Delay(100);
                }
            }

            return await _lockService.AcquireLockAsync<TResponse>(dynamicLockKey, lockTime, async () => await next());
        }
    }
}
