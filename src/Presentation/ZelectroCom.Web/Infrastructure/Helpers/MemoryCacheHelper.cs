using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace ZelectroCom.Web.Infrastructure.Helpers
{
    public static class MemoryCacheHelper
    {
        public struct CacheConsts
        {
            public const string OldMediaUrls = "OldMedia.Urls";
        }

        public static object OldMediaLockObj = new object();

        private static readonly Dictionary<string, object> LockObjectsDict = new Dictionary<string, object>()
        {
            {CacheConsts.OldMediaUrls, OldMediaLockObj}
        };

        private static readonly TimeSpan DefaultCacheInterval = new TimeSpan(0, 20, 0);

        public static T GetCachedData<T>(string cacheKey, Func<T> getData, 
            TimeSpan? cacheInterval = null, CacheItemPriority priority = CacheItemPriority.Normal)
            where T : class
        {
            cacheInterval = cacheInterval ?? DefaultCacheInterval;

            //Returns null if the string does not exist, prevents a race condition where the cache invalidates between the contains check and the retreival.
            T cachedData = HttpRuntime.Cache.Get(cacheKey) as T;

            if (cachedData != null)
            {
                return cachedData;
            }

            lock (LockObjectsDict[cacheKey])
            {
                //Check to see if anyone wrote to the cache while we where waiting our turn to write the new value.
                cachedData = HttpRuntime.Cache.Get(cacheKey) as T;

                if (cachedData != null)
                {
                    return cachedData;
                }

                //The value still did not exist so we now write it in to the cache.
                cachedData = getData();
                HttpRuntime.Cache.Add(cacheKey, cachedData, null, Cache.NoAbsoluteExpiration, cacheInterval.Value, priority, null);
                return cachedData;
            }
        }

        public static void RemoveCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }
    }
}
