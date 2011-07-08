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

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Helpers.UI
{
    public abstract class GrassrootsWebViewPage<TModel> : WebViewPage<TModel>
    {
        private readonly OrganizationBase organization;

        public string OrganizationName
        {
            get { return organization.Name; }
        }

        public string OrganizationTagline
        {
            get { return organization.Tagline; }
        }

        public string ContactEmail
        {
            get { return organization.ContactEmail; }
        }

        public string PublicWebsiteUrl
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.PUBLIC_WEBSITE_URL);
                return setting != null ? setting.Value : null;
            }
        }

        public string PublicAboutPageUrl
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.PUBLIC_ABOUT_PAGE_URL);
                return setting != null ? setting.Value : null;
            }
        }

        public string PublicServicesPageUrl
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.PUBLIC_SERVICES_PAGE_URL);
                return setting != null ? setting.Value : null;
            }
        }

        public string NavigationHtml
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.CUSTOM_NAVIGATION_HTML);
                return setting != null ? setting.Value : null;
            }
        }

        public string FooterHtml
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.CUSTOM_FOOTER_HTML);
                return setting != null ? setting.Value : null;
            }
        }

        public string HomePageHtml
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.CUSTOM_HOME_PAGE_HTML);
                return setting != null ? setting.Value : null;
            }
        }

        public string DonationInstructionsHtml
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.DONATE_INSTRUCTIONS_HTML);
                return setting != null ? setting.Value : null;
            }
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
            var repositoryFactory = new RepositoryFactory<IOrganizationRepository>();

            using (var organizationRepository = repositoryFactory.GetRepository())
            {
                organization = organizationRepository.GetDefaultOrganization(readOnly: true);
            }
        }
    }
}