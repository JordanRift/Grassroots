//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Data
{
    public class OrganizationRepository : GrassrootsRepositoryBase, IOrganizationRepository
    {
        private const string DEFAULT_ORG_CACHE_KEY = "Grassroots.DefaultOrganization";
        //private readonly SingletonCache instance;
        private ICache cache;

        public OrganizationRepository()
        {
            //instance = SingletonCache.Instance;
            cache = CacheFactory.GetCache();
        }

        public Organization GetOrganizationByID(int id)
        {
            return ObjectContext.Organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization(bool readOnly = true)
        {
            if (readOnly && cache.Any(i => i.Key == DEFAULT_ORG_CACHE_KEY))
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
