//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;
using System.Runtime.Caching;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Data
{
    public class OrganizationRepository : GrassrootsRepositoryBase, IOrganizationRepository
    {
        private const string DEFAULT_ORG_CACHE_KEY = "Grassroots.DefaultOrganization";
        private readonly SingletonCache instance;

        public OrganizationRepository()
        {
            instance = SingletonCache.Instance;
        }

        public Organization GetOrganizationByID(int id)
        {
            return ObjectContext.Organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization(bool readOnly = true)
        {
            if (readOnly && instance.Cache.Any(i => i.Key == DEFAULT_ORG_CACHE_KEY))
            {
                return instance.Cache.Get(DEFAULT_ORG_CACHE_KEY) as Organization;
            }

            var organization = ObjectContext.Organizations.FirstOrDefault();

            if (organization != null && readOnly)
            {
                instance.Cache.Add(new CacheItem(DEFAULT_ORG_CACHE_KEY, organization),
                                   new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(5) });
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
            instance.Cache.Remove(DEFAULT_ORG_CACHE_KEY);
        }
    }
}
