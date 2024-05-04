using System.Text.Json;
using CDS.Application.Common.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CDS.Infrastructure.Cache;

public class RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger, IConfiguration configuration) : ICacheService {
    static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };

    public void Set(string key, ICacheable cacheable, Type cacheableType) {
        string json = JsonSerializer.Serialize(cacheable, cacheableType, JsonSerializerOptions);
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

    public void Purge() {
        var connection = ConnectionMultiplexer.Connect(
            new ConfigurationOptions {
                EndPoints = { configuration.GetConnectionString("Redis")! },
                AllowAdmin = true
            }
        );

        var server = connection.GetServer(connection.GetEndPoints().First());
        server.FlushDatabase();
    }
}