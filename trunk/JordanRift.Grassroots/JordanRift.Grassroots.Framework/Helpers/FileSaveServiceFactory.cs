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
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public class FileSaveServiceFactory
    {
        private static IEnumerable<IFileSaveService> services;

        public FileStorageType StorageMode { get; set; }

        private IFileSaveService FileStorageImplementation
        {
            get { return services.Where(s => s.StorageMode == this.StorageMode).First(); }
        }

        static FileSaveServiceFactory()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IFileSaveService).Assembly));
            CompositionContainer container = new CompositionContainer(catalog);
            services = container.GetExportedValues<IFileSaveService>();
        }

        public FileSaveServiceFactory()
        {
            var config = ConfigurationManager.AppSettings["FileStorageType"];

            if (config != null)
            {
                StorageMode = (FileStorageType) Enum.Parse(typeof (FileStorageType), config);
            }
            else
            {
                StorageMode = FileStorageType.FileSystem;
            }
        }

        public IFileSaveService GetService()
        {
            return FileStorageImplementation;
        }
    }
}
