using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Infrastructure.Redis
{
    public class RedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheValueAsync(string key, string value)
        {
            await _cache.SetStringAsync(key, value);
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            return await _cache.GetStringAsync(key);
        }
    }
}
