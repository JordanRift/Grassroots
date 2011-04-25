//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Runtime.Caching;

namespace JordanRift.Grassroots.Framework.Helpers
{
    /// <summary>
    /// Singleton implementation to act as a wrapper for a MemoryCache object.
    /// </summary>
    public class SingletonCache
    {
        // Lazy<T> in this usage does act as a thread-safe instance of the generic type. No need for "volatile".
        private static readonly Lazy<SingletonCache> instance = new Lazy<SingletonCache>(() => new SingletonCache());

        public static SingletonCache Instance
        {
            get { return instance.Value; }
        }

        private MemoryCache cache;

        public MemoryCache Cache
        {
            get { return cache ?? (cache = MemoryCache.Default); }
        }

        private SingletonCache() { }
    }
}
