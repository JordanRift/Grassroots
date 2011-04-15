//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Controllers
{
    public abstract class GrassrootsControllerBase : Controller
    {
        private IOrganizationRepository organizationRepository;
        private Organization organization;

        public IOrganizationRepository OrganizationRepository 
        { 
            get
            {
                if (organizationRepository == null)
                {
                    organizationRepository = RepositoryFactory.GetRepository<IOrganizationRepository>();
                }

                return organizationRepository;
            } 
            
            set { organizationRepository = value; }
        }

        public Organization Organization
        {
            get
            {
                if (organization == null)
                {
                    organization = OrganizationRepository.GetDefaultOrganization();
                }

                return organization;
            }

            set { organization = value; }
        }
    }
}
