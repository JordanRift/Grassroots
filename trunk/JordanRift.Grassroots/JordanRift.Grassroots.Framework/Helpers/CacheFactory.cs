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

using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public class CacheFactory
    {
        private static IEnumerable<ICache> caches;
        
        public CacheType StorageMode { get; set; }

        private ICache CachingImplementation
        {
            get { return caches.Where(c => c.Type == StorageMode).First(); }
        }

        static CacheFactory()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ICache).Assembly));
            CompositionContainer container = new CompositionContainer(catalog);
            caches = container.GetExportedValues<ICache>();
        }

        public CacheFactory()
        {
            // TODO: Read in setting from config file.
            StorageMode = CacheType.HttpContext;
        }

        public ICache GetCache()
        {
            return CachingImplementation;
        }
    }
}
