//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Helpers.UI
{
    public abstract class GrassrootsWebViewPage<TModel> : WebViewPage<TModel>
    {
        private readonly IOrganizationRepository organizationRepository;
        private readonly Organization organization;

        public string OrganizationName
        {
            get { return organization.Name; }
        }

        public string OrganizationTagline
        {
            get { return organization.Tagline; }
        }

        public string AnalyticsTrackingCode
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.ANALYTICS_TRACKING_CODE);
                return setting != null ? setting.Value : null;
            }
        }

        protected GrassrootsWebViewPage()
        {
            organizationRepository = RepositoryFactory.GetRepository<IOrganizationRepository>();
            organization = organizationRepository.GetDefaultOrganization(readOnly: true);
        }
    }
}