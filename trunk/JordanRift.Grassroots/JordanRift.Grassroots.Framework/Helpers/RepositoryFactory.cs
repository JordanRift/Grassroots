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

namespace JordanRift.Grassroots.Framework.Helpers
{
    public class RepositoryFactory<TRepository>
    {
        private readonly IEnumerable<TRepository> repositories;

        private TRepository RepositoryImplementation
        {
            get
            {
                try
                {
                    return repositories.OrderByDescending(r => ((IPriority)r).Priority).First();
                }
                catch
                {
                    return repositories.First();
                }
            }
        }

        public RepositoryFactory()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TRepository).Assembly));
            CompositionContainer container = new CompositionContainer(catalog);
            repositories = container.GetExportedValues<TRepository>();
        }

        public TRepository GetRepository()
        {
            bool isTest;
            var setting = ConfigurationManager.AppSettings["RepositoryTestMode"];
            bool.TryParse(setting, out isTest);

            if (isTest)
            {
                return LoadFromConfig();
            }

            return RepositoryImplementation;
        }

        private static TRepository LoadFromConfig()
        {
            var className = typeof(TRepository).ToString().Split(new[] { '.' }).LastOrDefault();

            if (string.IsNullOrEmpty(className))
            {
                throw new InvalidOperationException();
            }

            var setting = ConfigurationManager.AppSettings[className];
            var settingArray = setting.Split(new[] { ',' });
            var classPath = settingArray[0].Trim();
            var assemblyName = settingArray[1].Trim();
            return (TRepository) Activator.CreateInstance(assemblyName, classPath).Unwrap();
        }
    }
}
