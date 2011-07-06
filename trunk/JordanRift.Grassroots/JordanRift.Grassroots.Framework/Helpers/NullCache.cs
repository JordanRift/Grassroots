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

using System.ComponentModel.Composition;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Helpers
{
    [Export(typeof(ICache))]
    public class NullCache : ICache
    {
        public CacheType Type
        {
            get { return CacheType.Null; }
        }

        public object Get(string key)
        {
            return null;
        }

        public void Add(string key, object value)
        {
        }

        public void Remove(string key)
        {
        }

        //public bool Any(Func<KeyValuePair<string, object>, bool> predicate)
        //{
        //    return false;
        //}
    }
}
