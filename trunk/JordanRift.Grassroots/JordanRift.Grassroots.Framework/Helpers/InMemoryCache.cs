﻿//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
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
