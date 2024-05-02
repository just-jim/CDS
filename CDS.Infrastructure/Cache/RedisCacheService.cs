using System.Text.Json;
using CDS.Application.Common.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.Cache;

public class RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger) : ICacheService {
    static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };
    
    public void Set(string key, ICacheable cacheable) {
        string json = JsonSerializer.Serialize(cacheable, JsonSerializerOptions);
        cache.SetString(key, json);
    }
        
    public ICacheable? Get(string key, Type cacheableType) {
        string? json = cache.GetString(key);
        
        try {
            return (ICacheable?)JsonSerializer.Deserialize(json, cacheableType, JsonSerializerOptions);
        }
        catch (ArgumentNullException) {
            return null;
        } 
        catch (Exception e) {
            logger.LogError($"Failed to deserialize the cached object of type {cacheableType.Name} with key {key}", e);
        }

        return null;
    }
}