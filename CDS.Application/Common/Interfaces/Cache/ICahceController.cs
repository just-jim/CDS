namespace CDS.Application.Common.Interfaces.Cache;

public interface ICacheService {
    public ICacheable? Get(string key, Type cacheableType);
    public void Set(string key, ICacheable cacheable, Type cacheableType);
}