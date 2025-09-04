using System;
using System.Threading.Tasks;

namespace Application.Interfaces;

/// <summary>
/// Interface for caching service.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Gets a value from the cache.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <returns>The cached value, or default(T) if not found.</returns>
    T? Get<T>(string key);

    /// <summary>
    /// Sets a value in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    void Set<T>(string key, T value, TimeSpan expiration);

    /// <summary>
    /// Removes a value from the cache.
    /// </summary>
    /// <param name="key">The cache key.</param>
    void Remove(string key);

    /// <summary>
    /// Gets a value from the cache or creates it if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">The factory function to create the value if it doesn't exist.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    /// <returns>The cached or newly created value.</returns>
    T GetOrCreate<T>(string key, Func<T> factory, TimeSpan expiration);

    /// <summary>
    /// Gets a value from the cache or creates it if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">The factory function to create the value if it doesn't exist.</param>
    /// <param name="expiration">The expiration time for the cache entry.</param>
    /// <returns>The cached or newly created value.</returns>
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration);
}