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
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Data
{
    [Export(typeof(IOrganizationRepository))]
    public class OrganizationRepository : GrassrootsRepositoryBase, IOrganizationRepository
    {
        private const string DEFAULT_ORG_CACHE_KEY = "Grassroots.DefaultOrganization";
        private readonly CacheManager cacheManager;

        public OrganizationRepository()
        {
            Priority = PriorityType.Low;
            cacheManager = new CacheManager();
        }

        public Organization GetOrganizationByID(int id)
        {
            return ObjectContext.Organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization(bool readOnly = true)
        {
            Organization organization;

            if (cacheManager.Exists(DEFAULT_ORG_CACHE_KEY))
            {
                organization = cacheManager.Get<Organization>(DEFAULT_ORG_CACHE_KEY);

                if (ObjectContext.Entry(organization).State == EntityState.Detached)
                {
                    try
                    {
                        ObjectContext.Organizations.Attach(organization);
                        ObjectContext.Entry(organization).State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException) { }
                }

                return organization;
            }

            organization = ObjectContext.Organizations.FirstOrDefault();

            if (organization != null)
            {
                cacheManager.Add(DEFAULT_ORG_CACHE_KEY, organization);
                ObjectContext.Entry(organization).State = EntityState.Detached;
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

        public void DeleteSetting(OrganizationSetting organizationSetting)
        {
            ObjectContext.OrganizationSettings.Remove(organizationSetting);
        }

        void IOrganizationRepository.Save()
        {
            base.Save();
            cacheManager.Remove(DEFAULT_ORG_CACHE_KEY, removingOrganization: true);
        }
    }
}
