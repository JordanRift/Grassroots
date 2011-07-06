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
using System.Linq;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Data
{
    [Export(typeof(IOrganizationRepository))]
    public class OrganizationRepository : GrassrootsRepositoryBase, IOrganizationRepository
    {
        private const string DEFAULT_ORG_CACHE_KEY = "Grassroots.DefaultOrganization";
        private ICache cache;

        public OrganizationRepository()
        {
            var cacheFactory = new CacheFactory();
            cache = cacheFactory.GetCache();
            Priority = PriorityType.Low;
        }

        public Organization GetOrganizationByID(int id)
        {
            return ObjectContext.Organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization(bool readOnly = true)
        {
            if (readOnly && cache.Get(DEFAULT_ORG_CACHE_KEY) != null)
            {
                return cache.Get(DEFAULT_ORG_CACHE_KEY) as Organization;
            }

            var organization = ObjectContext.Organizations.FirstOrDefault();

            if (organization != null && readOnly)
            {
                cache.Add(DEFAULT_ORG_CACHE_KEY, organization);
            }

            return organization;
        }

        public void Add(Organization organization)
        {
            ObjectContext.Organizations.Add(organization);
        }

        public void Delete(Organization organization)
        {
            ObjectContext.Organizations.Remove(organization);
        }

        void IOrganizationRepository.Save()
        {
            base.Save();
            ClearCache();
        }

        private void ClearCache()
        {
            cache.Remove(DEFAULT_ORG_CACHE_KEY);
        }
    }
}
