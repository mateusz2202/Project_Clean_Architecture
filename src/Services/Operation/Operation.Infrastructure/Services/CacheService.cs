using Operation.Application.Contracts.Services;
using StackExchange.Redis;
using System.Text.Json;


namespace Operation.Infrastructure.Services;
public class CacheService : ICacheService
{
    IDatabase _cache;

    public CacheService(IDatabase cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _cache.StringGetAsync(key);
        var json = data.ToString();
        if (!string.IsNullOrEmpty(data))
            return JsonSerializer.Deserialize<T>(json);
        return default;
    }
    public async Task<bool> SetAsync<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = await _cache.StringSetAsync(key, JsonSerializer.Serialize(value), expirtyTime);
        return isSet;
    }
    public async Task<bool> RemoveAsync(string key)
    {
        var exist = _cache.KeyExists(key);
        if (exist)
            return await _cache.KeyDeleteAsync(key);
        return false;
    }
}