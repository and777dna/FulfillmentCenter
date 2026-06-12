using FulfillmentCenter.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FulfillmentCenter.Services.Implementations;

public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    private readonly IMemoryCache _cache = cache;

    public bool TryGet<T>(string key, out T? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Set<T>(string key, T value, TimeSpan ttl)
    {
        _cache.Set(key, value, ttl);
    }
}