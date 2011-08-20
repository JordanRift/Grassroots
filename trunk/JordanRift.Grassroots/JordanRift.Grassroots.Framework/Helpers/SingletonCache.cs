//
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
using System.Runtime.Caching;

namespace JordanRift.Grassroots.Framework.Helpers
{
    /// <summary>
    /// Singleton implementation to act as a wrapper for a MemoryCache object.
    /// </summary>
    internal sealed class SingletonCache
    {
        // Lazy<T> in this usage does act as a thread-safe instance of the generic type. No need for "volatile".
        private static readonly Lazy<SingletonCache> instance = new Lazy<SingletonCache>(() => new SingletonCache());

        public static SingletonCache Instance
        {
            get { return instance.Value; }
        }

        private ObjectCache cache;

        public ObjectCache Cache
        {
            get { return cache ?? (cache = MemoryCache.Default); }
        }

        private SingletonCache() { }
    }
}
