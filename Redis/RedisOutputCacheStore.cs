using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;
using OrchardCore.Redis;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etch.OrchardCore.OutputCache.Redis
{
    public class RedisOutputCacheStore : IOutputCacheStore
    {
        private readonly IRedisService _redis;
        private readonly ILogger<RedisOutputCacheStore> _logger;

        private readonly string _prefix;

        public RedisOutputCacheStore(
            ILogger<RedisOutputCacheStore> logger,
            IRedisService redis,
            ShellSettings shellSettings)
        {
            _logger = logger;
            _redis = redis;

            _prefix = redis.InstancePrefix + shellSettings.Name + ":OutputCache:";
        }

        public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            await ConnectAsync();

            await _redis.Database.KeyDeleteAsync(
                _redis.Database
                    .SetMembers($"{_prefix}tag_{tag}")
                    .Select(x => x.ToString())
                    .Select(x => new RedisKey(x))
                    .ToArray()
            );
        }

        public async ValueTask<byte[]> GetAsync(string key, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key);

            await ConnectAsync();

            var entry = await _redis.Database.StringGetAsync(new RedisKey(_prefix + key));

            if (!entry.HasValue)
            {
                return default(byte[]);
            }

            return Encoding.Default.GetBytes(entry);
        }

        public async ValueTask SetAsync(string key, byte[] value, string[] tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(value);

            await ConnectAsync();

            await _redis.Database.StringSetAsync(new RedisKey(_prefix + key), new RedisValue(Encoding.Default.GetString(value)), validFor);

            foreach (var tag in tags)
            {
                await _redis.Database.SetAddAsync($"{_prefix}tag_{tag}", new RedisValue(_prefix + key));
            }
        }

        private async Task ConnectAsync()
        {
            if (_redis.Database == null)
            {
                await _redis.ConnectAsync();

                if (_redis.Database == null)
                {
                    _logger.LogError($"Failed to connect to Redis for output caching.");
                    return;
                }
            }
        }
    }
}