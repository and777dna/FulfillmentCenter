using FulfillmentCenter.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FulfillmentCenter.Services.Implementations;

public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    public bool TryGet<T>(string key, out T? value)
    {
        return cache.TryGetValue(key, out value);
    }

    public void Set<T>(string key, T value, TimeSpan ttl)
    {
        cache.Set(key, value, ttl);
    }
}