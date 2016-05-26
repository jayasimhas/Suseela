using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace Informa.Library.Caching {
    public interface ICacheProvider
    {
        bool IsInCache(string key);

        void SetObject(string cacheKey, object o, DateTime expiration);

        void SetObject(string cacheKey, object o, CacheDependency dependency, DateTime expiration,
            TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback callback);

        T GetObject<T>(string key);

        void ClearObject(string key);
    }
}
