//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using System.Runtime.Caching;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public class OrganizationRepository : GrassrootsRepositoryBase, IOrganizationRepository
    {
        private static ObjectCache cache;
        private const string DEFAULT_ORG_CACHE_KEY = "MyC.DefaultOrganization";

        public OrganizationRepository()
        {
            cache = MemoryCache.Default;
        }

        public Organization GetOrganizationByID(int id)
        {
            return ObjectContext.Organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization()
        {
            //if (cache.Any(i => i.Key == DEFAULT_ORG_CACHE_KEY))
            //{
            //    return cache.Get(DEFAULT_ORG_CACHE_KEY) as Organization;
            //}

            var organization = ObjectContext.Organizations.FirstOrDefault();

            //if (organization != null)
            //{
            //    cache.Add(new CacheItem(DEFAULT_ORG_CACHE_KEY, organization), 
            //        new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(5) });
            //}

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

        private static void ClearCache()
        {
            cache.Remove(DEFAULT_ORG_CACHE_KEY);
        }
    }
}
