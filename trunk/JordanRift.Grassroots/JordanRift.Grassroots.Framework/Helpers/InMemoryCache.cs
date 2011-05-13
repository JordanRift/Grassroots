//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace JordanRift.Grassroots.Framework.Helpers
{
    /// <summary>
    /// Facade/wrapper for singleton cache implementation
    /// </summary>
    public sealed class InMemoryCache : ICache
    {
        private readonly SingletonCache instance;

        public InMemoryCache()
        {
            instance = SingletonCache.Instance;
        }

        public object Get(string key)
        {
            return instance.Cache.Get(key);
        }

        public void Add(string key, object value)
        {
            instance.Cache.Add(new CacheItem(key, value),
                               new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(5) });
        }

        public void Remove(string key)
        {
            instance.Cache.Remove(key);
        }

        public bool Any(Func<KeyValuePair<string, object>, bool> predicate)
        {
            return instance.Cache.Any(predicate);
        }
    }
}
