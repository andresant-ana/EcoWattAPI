using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace EcoWatt.API.Services
{
    public interface IConfigCacheService
    {
        string GetConfigValue(string key);
    }

    public class ConfigCacheService : IConfigCacheService
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, string> _cache;

        public ConfigCacheService(IConfiguration configuration)
        {
            _configuration = configuration;
            _cache = new ConcurrentDictionary<string, string>();
        }

        public string GetConfigValue(string key)
        {
            return _cache.GetOrAdd(key, _configuration[key] ?? string.Empty);
        }
    }
}