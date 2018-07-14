using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace CachingKeyValue.Logic
{
    public class KeyValueCache : IKeyValueCache
    {
        public object GetValue(string key)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                return memoryCache.Get(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("KeyValueCcahe.GetValue " + ex);
            }
            return null;
        }

        public bool Add(string key, object value, double timeInSecs)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(timeInSecs);           
                return memoryCache.Add(key, value, policy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("KeyValueCcahe.Add " + ex);
            }
            return false;
        }

        public bool Delete(string key)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                if (memoryCache.Contains(key))
                {
                    memoryCache.Remove(key);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("KeyValueCcahe.Delete " + ex);
            }
            return false;
        }
    }
}