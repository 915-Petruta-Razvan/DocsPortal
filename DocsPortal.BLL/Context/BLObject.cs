using Microsoft.Extensions.Caching.Distributed;

namespace DocsPortal.BLL.Context
{
    public class BLObject
    {
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        protected BLContext Context;

        public BLObject(BLContext context)
        {
            this.Context = context;
        }

        protected string GetCacheKey(string prefix, string id) => $"{prefix}:{id}";
        protected string GetCacheKey(string prefix, Guid id) => $"{prefix}:{id}";

        protected string? GetRedisValue(string key)
        {
            return Context.Cache.GetString(key);
        }

        protected void SetRedisValue(string key, string value)
        {
            Context.Cache.SetString(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiration
            });
        }
    }
}
