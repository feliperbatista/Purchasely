using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Purchasely.Application.Interfaces;
using StackExchange.Redis;

namespace Purchasely.Infrastructure.Services;

public class RedisCacheService(
    IDistributedCache cache,
    IConnectionMultiplexer redis
) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await cache.GetStringAsync(key, cancellationToken);
        if (data is null) return default;
        return JsonSerializer.Deserialize<T>(data, JsonOptions);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var server = redis.GetServer(redis.GetEndPoints().First());
        var keys = server.Keys(pattern: $"{prefix}*").ToArray();

        if (keys.Length == 0) return;

        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(keys);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
        };

        var data = JsonSerializer.Serialize(value, JsonOptions);
        await cache.SetStringAsync(key, data, options, cancellationToken);
    }
}