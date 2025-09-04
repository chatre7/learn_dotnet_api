using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services;

/// <summary>
/// Implementation of ICacheService using MemoryCache.
/// </summary>
public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class.
    /// </summary>
    /// <param name="memoryCache">The memory cache instance.</param>
    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Gets a value from the cache.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <returns>The cached value, or default(T) if not found.</returns>
    public T? Get<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return value;
    }

    /// <summary>
    /// Sets a value in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expiration);

        _memoryCache.Set(key, value, cacheEntryOptions);
    }

    /// <summary>
    /// Removes a value from the cache.
    /// </summary>
    /// <param name="key">The cache key.</param>
    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }

    /// <summary>
    /// Gets a value from the cache or creates it if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">The factory function to create the value if it doesn't exist.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    /// <returns>The cached or newly created value.</returns>
    public T GetOrCreate<T>(string key, Func<T> factory, TimeSpan expiration)
    {
        if (_memoryCache.TryGetValue(key, out T? existingValue))
        {
            return existingValue!;
        }

        var newValue = factory();
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expiration);

        _memoryCache.Set(key, newValue, cacheEntryOptions);
        return newValue;
    }

    /// <summary>
    /// Gets a value from the cache or creates it if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">The factory function to create the value if it doesn't exist.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    /// <returns>The cached or newly created value.</returns>
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
    {
        if (_memoryCache.TryGetValue(key, out T? existingValue))
        {
            return existingValue!;
        }

        var newValue = await factory();
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expiration);

        _memoryCache.Set(key, newValue, cacheEntryOptions);
        return newValue;
    }
}