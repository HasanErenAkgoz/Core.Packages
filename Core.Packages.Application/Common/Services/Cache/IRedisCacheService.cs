﻿using Core.Packages.Application.Shared.Result;

namespace Core.Packages.Application.Common.Services.Cache
{
    public interface IRedisCacheService
    {
        Task SetJsonAsync<T>(string key, T data, TimeSpan expiration);
        Task<T?> GetJsonAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}
