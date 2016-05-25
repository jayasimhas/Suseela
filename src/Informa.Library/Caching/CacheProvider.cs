using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Caching {

    [AutowireService]
    public class CacheProvider : ICacheProvider {
        
        Cache CacheState
        {
            get
            {
                return (HttpContext.Current == null) ? null : HttpContext.Current.Cache;
            }
        }

        #region Cache Functions

        /// <summary>
        /// checks if an object is in cache or not
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public bool IsInCache(string key) {
            return (CacheState != null && CacheState[key] != null);
        }

        public void SetObject(string cacheKey, object o, DateTime expiration) {
            SetObject(cacheKey, o, null, expiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// caches items based on parameters give
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="o"></param>
        /// <param name="dependency"></param>
        /// <param name="expiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="priority"></param>
        /// <param name="callback"></param>
        public void SetObject(string cacheKey, object o, CacheDependency dependency, DateTime expiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback callback)
        {
            CacheState?.Add(cacheKey, o, dependency, expiration, slidingExpiration, priority, callback);
        }

        /// <summary>
        /// retrieves an object from cache given the cache key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public T GetObject<T>(string key)
        {
            if (CacheState == null)
                return default(T);

            object o = CacheState[key];
            return (string.IsNullOrEmpty(key) == false && o != null && o is T)
                ? (T)o
                : default(T);
        }

        public void ClearObject(string key) {
            CacheState?.Remove(key);
        }

        #endregion Cache Functions
    }
}
