using System.Text.Json;
using Back_Quiz.Interfaces;
using StackExchange.Redis;

namespace Back_Quiz.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    
    public RedisService(IDatabase db)
    {
        _db = db;
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, ttl);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (!value.HasValue)
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _db.KeyExistsAsync(key);
    }
}