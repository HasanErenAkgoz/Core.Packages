namespace Core.Packages.Application.Common.Attributies
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CacheAttribute : Attribute
    {
        public string CacheKey { get; }
        public int ExpiryMinutes { get; }
        public bool InvalidateOnChange { get; }

        public CacheAttribute(string cacheKey, int expiryMinutes = 30, bool invalidateOnChange = false)
        {
            CacheKey = cacheKey;
            ExpiryMinutes = expiryMinutes;
            InvalidateOnChange = invalidateOnChange;
        }
    }
}
