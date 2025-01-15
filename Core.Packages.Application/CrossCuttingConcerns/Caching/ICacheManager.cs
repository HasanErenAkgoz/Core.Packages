namespace Core.Packages.Application.CrossCuttingConcerns.Caching;

public interface ICacheManager
{
    T Get<T>(string key);
    object Get(string key);
    void Add(string key, object value, int duration);
    void Add(string key, object value, DateTime expireTime);
    bool IsAdd(string key);
    void Remove(string key);
    void RemoveByPattern(string pattern);
    void Clear();
} 